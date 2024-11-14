using Azure;
using System.Drawing.Printing;
using Todo.API.Dtos;
using Todo.API.Helpers;
using Todo.API.Helpers.Mappers;
using Todo.API.Intefaces;
using Todo.API.Models.Enums;
using Todo.API.Respositories;
using Todo.API.Validators;

namespace Todo.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IValidator<CreateUserDto> _userCreateValidator;
        private readonly IValidator<UpdateUserDto> _userUpdateValidator;

        public UserService
            (IUserRepository userRepository, 
            IValidator<CreateUserDto> userCreatedValidator, 
            IValidator<UpdateUserDto> userUpdateValidator)
        {
            _userRepository = userRepository;
            _userCreateValidator = userCreatedValidator;
            _userUpdateValidator = userUpdateValidator;
        }

        public async Task<ApiResponse> CreateUserAsync(CreateUserDto createUserDto, CancellationToken token = default)
        {
            ApiResponse response = new();
            
            _userCreateValidator.ValidateAndThrow(createUserDto);

            UserModel? existUser = await _userRepository.GetUserByEmailAsync(createUserDto.Email);

            if(existUser is not null)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.UserAlreadyExists;
                return response;
            }

            (string passwordHash, string Salt) = FunctionHelpers.HashPassword(createUserDto.Password);

            var user = createUserDto.MapCreateUserDtoToUserModel(passwordHash, Salt);

            int userId = await _userRepository.CreateUserAsync(user, token);

            response.Code = ApiErrorCode.Failed;
            response.Message = Utils.ErrorMessage.Failed;

            if (userId > 0)
            {
                response.Code = ApiErrorCode.Success;
                response.Message = Utils.ErrorMessage.Success;

                response.Params.Add(new Param { Name = "UserId", Value = userId.ToString() });
            }

            return response;
        }

        public async Task<ApiResponse> UpdateUserAsync(UpdateUserDto updateUserDto, CancellationToken token = default)
        {
            ApiResponse response = new();
         
            _userUpdateValidator.ValidateAndThrow(updateUserDto);
            
            UserModel? user = await _userRepository.GetUserByIdAsync(updateUserDto.Id, token);

            if(user is null)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.UserNotFound;
                return response;
            }
            
            if(updateUserDto.Password is not null)
            {
               (string password, string Salt) =  FunctionHelpers.HashPassword(updateUserDto.Password);
                user.Password = password;
                user.Salt = Salt;
            }

            user.FullName = updateUserDto.FullName;
            user.Role = updateUserDto.Role;

          
            var result = await _userRepository.UpdateUserAsync(user);

            response.Code = ApiErrorCode.Failed;
            response.Message = Utils.ErrorMessage.Failed;

            if (result)
            {
                response.Code = ApiErrorCode.Success;
                response.Message = Utils.ErrorMessage.Success;

                response.Params.Add(new Param { Name = "UserId", Value = user.Id.ToString() });
            }

            return response;
        }

        public async Task<ApiResponse> DeleteUserAsync(int id, CancellationToken token = default)
        {
            ApiResponse response = new();
           
            if (id >= 0)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.InvalidUserId;
            }

            UserModel? user = await _userRepository.GetUserByIdAsync(id, token);

            if(user is null)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.UserNotFound;
                return response;
            }

            var result = await _userRepository.DeleteUserAsync(user, token);

            response.Code = ApiErrorCode.Failed;
            response.Message = Utils.ErrorMessage.Failed;

            if (result)
            {
                response.Code = ApiErrorCode.Success;
                response.Message = Utils.ErrorMessage.Success;
            }
           
            return response;
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int id ,CancellationToken token = default)
        {
            UserModel? user =  await _userRepository.GetUserByIdAsync(id, token);

            if(user is null )
                return null;

            return user.MapUserModelToUserDto();
        }
            

        public async Task<PaginationResponse<UserResponseDto>> GetUsersAsync(UserQueryOptions userQueryOptions, CancellationToken token = default)
        {
            PaginationResponse<UserResponseDto> response = new();

            userQueryOptions.Limit = userQueryOptions.ItemsOfPage;
            userQueryOptions.Skip = userQueryOptions.Page == 1 ? 0 : (userQueryOptions.Page - 1) * userQueryOptions.Limit;

            IEnumerable<UserModel> users = await _userRepository.GetUsersAsync(userQueryOptions, token);

            IEnumerable<UserResponseDto> usersDto = users.MapUsersModelToUsersDto();

            int countUsers = await _userRepository.GetCountUsersCountAsync(userQueryOptions, token);

            response.Enttities = usersDto;
            response.CountPage = (int)Math.Ceiling(countUsers / (decimal)userQueryOptions.ItemsOfPage!);
            response.CurrentPage = userQueryOptions.Page;

            return response;
        }

    }
}
