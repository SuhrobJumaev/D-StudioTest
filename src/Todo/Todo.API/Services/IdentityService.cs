using Azure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Todo.API.Dtos;
using Todo.API.Helpers;
using Todo.API.Intefaces;
using Todo.API.Models.Enums;
using Todo.API.Models.SettingModels;
using Todo.API.Respositories;

namespace Todo.API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private JwtSettingsModel _jwtOptions;

        public IdentityService(IUserRepository userRepository, IOptions<JwtSettingsModel> jwtOptions)
        {
            _userRepository = userRepository;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<ApiResponse> Login(LoginDto loginDto, CancellationToken token = default)
        {
            ApiResponse response = new();

            

            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email,token);

            if (user is null)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.UserNotFound;
                return response;
            }

            bool isValidPassword = FunctionHelpers.VerifyPassword(loginDto.Password, user.Password, user.Salt);

            if (!isValidPassword) 
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.InvallidUserEmailOrPassword;
                return response;
            }

            string jwtToken = GenerateJwtToken(user);

            var tokenExpiryDate = DateTime.Now.AddHours(_jwtOptions.AccessTokenExpirationMinutes);

            var refreshToken = GenerateRefreshToken();
            var hashedRefreshToken = HashToken(refreshToken);

            RefreshTokenModel refreshTokenModel = new()
            {
                Token = hashedRefreshToken,
                ExpiryDate = DateTime.Now.AddHours(_jwtOptions.RefreshTokenExpirationHours),
                UserId = user.Id
            };

            await _userRepository.RevokedAllUserRefreshTokensAsync(user.Id);

            await _userRepository.SaveRefreshTokenAsync(refreshTokenModel);

            response.Code = ApiErrorCode.Success;
            response.Message = Utils.ErrorMessage.Success;

            response.Params.Add(new Param { Name = "AccessToken", Value = jwtToken });
            response.Params.Add(new Param { Name = "RefreshToken", Value = refreshToken });

            return response;
        }

        public async Task<ApiResponse> RefreshToken(RefreshTokenDto refreshDto, CancellationToken token = default)
        {
            ApiResponse response = new();

            if (refreshDto.UserId <= 0)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message =  Utils.ErrorMessage.InvalidUserId;
                return response;
            }

            UserModel? user = await _userRepository.GetUserByIdAsync(refreshDto.UserId, token);

            if (user is null)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.UserNotFound;
                return response;
            }

            var hashedToken = HashToken(refreshDto.RefreshToken);

            RefreshTokenModel? refreshToken = await _userRepository.GetRefreshTokenByUserIdAndTokenAsync(refreshDto.UserId, hashedToken, token);

            if (refreshToken is null)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.RefreshTokenNotFound;
                return response;
            }

            if (refreshToken.ExpiryDate < DateTime.Now)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.RefreshTokenExpiried;
                return response;
            }

            string newAccessToken = GenerateJwtToken(user);
            string newRefreshToken = GenerateRefreshToken();

            string hashedRefreshToken = HashToken(newRefreshToken);

            RefreshTokenModel refreshTokenModel = new()
            {
                Token = hashedRefreshToken,
                ExpiryDate = DateTime.Now.AddHours(_jwtOptions.RefreshTokenExpirationHours),
                UserId = user.Id
            };

            await _userRepository.RevokedAllUserRefreshTokensAsync(user.Id);

            await _userRepository.SaveRefreshTokenAsync(refreshTokenModel);

            response.Code = ApiErrorCode.Success;
            response.Message = Utils.ErrorMessage.Success;

            response.Params.Add(new Param { Name = "AccessToken", Value = newAccessToken });
            response.Params.Add(new Param { Name = "RefreshToken", Value = newRefreshToken });

            return response;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var numberGenerator = RandomNumberGenerator.Create();

            numberGenerator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateJwtToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("UserId", user.Id.ToString()),
            new("Email", user.Email),
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var Securetoken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(Securetoken);
        }
        private string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }
}
