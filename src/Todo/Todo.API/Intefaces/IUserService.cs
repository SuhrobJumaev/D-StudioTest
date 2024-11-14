namespace Todo.API.Intefaces
{
    public interface IUserService
    {
        Task<ApiResponse> CreateUserAsync(CreateUserDto createUserDto, CancellationToken token = default);
        Task<ApiResponse> UpdateUserAsync(UpdateUserDto updateUserDto, CancellationToken token = default);
        Task<ApiResponse> DeleteUserAsync(int id, CancellationToken token = default);
        Task<UserResponseDto?> GetUserByIdAsync(int id, CancellationToken token = default);
        Task<PaginationResponse<UserResponseDto>> GetUsersAsync(UserQueryOptions userQueryOptions, CancellationToken token = default);
    }
}
