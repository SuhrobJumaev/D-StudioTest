namespace Todo.API.Intefaces
{
    public interface IUserRepository
    {
        Task<UserModel?> GetUserByEmailAsync(string email, CancellationToken token = default);
        Task<bool> RevokedAllUserRefreshTokensAsync(int userId, CancellationToken token = default);
        Task<bool> SaveRefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken token = default);
        Task<UserModel?> GetUserByIdAsync(int userId, CancellationToken token = default);
        Task<RefreshTokenModel?> GetRefreshTokenByUserIdAndTokenAsync(int userId, string refreshToken, CancellationToken token = default);
        Task<int> CreateUserAsync(UserModel user, CancellationToken token = default);
        Task<bool> UpdateUserAsync(UserModel user, CancellationToken token = default);
        Task<bool> DeleteUserAsync(UserModel user, CancellationToken token = default);
        Task<List<UserModel>> GetUsersAsync(UserQueryOptions queryOptions, CancellationToken token = default);
        Task<int> GetCountUsersCountAsync(UserQueryOptions queryOptions, CancellationToken token = default);
    }
}
