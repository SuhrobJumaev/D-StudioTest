using Todo.API.Dtos;

namespace Todo.API.Intefaces
{
    public interface IIdentityService
    {
        Task<ApiResponse> Login(LoginDto loginDto, CancellationToken token = default);
        Task<ApiResponse> RefreshToken(RefreshTokenDto refreshDto, CancellationToken token = default);
    }
}
