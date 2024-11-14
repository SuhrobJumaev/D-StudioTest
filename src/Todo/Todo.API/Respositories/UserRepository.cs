using Todo.API.Dtos;
using Todo.API.Intefaces;

namespace Todo.API.Respositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email, CancellationToken token = default) 
        {
            return await _context.Users
                                .Where(u => u.Email == email)
                                .FirstOrDefaultAsync(token);
        }

        public async Task<bool> RevokedAllUserRefreshTokensAsync(int userId, CancellationToken token = default)
        {
            RefreshTokenModel? refreshTokenModel = await _context.RefreshTokens.
                    Where(t => t.UserId == userId && t.IsRevoked == false)
                    .FirstOrDefaultAsync(token);

            if (refreshTokenModel is null)
                return false;

            
            refreshTokenModel.IsRevoked = true;

            await _context.SaveChangesAsync(token);

            return true;
        }

        public async Task<bool> SaveRefreshTokenAsync(RefreshTokenModel refreshTokenModel, CancellationToken token = default)
        {
            await _context.RefreshTokens.AddAsync(refreshTokenModel, token);

            return await _context.SaveChangesAsync(token) > 0;
        }

        public async Task<UserModel?> GetUserByIdAsync(int userId, CancellationToken token = default)
        {
            return await _context.Users
                                    .Where(u => u.Id == userId)
                                    .FirstOrDefaultAsync(token);
        }

     
        public async Task<RefreshTokenModel?> GetRefreshTokenByUserIdAndTokenAsync(int userId, string refreshToken, CancellationToken token = default)
        {
            return await _context.RefreshTokens
                                        .Where(t => t.UserId == userId && t.Token == refreshToken)
                                        .FirstOrDefaultAsync(token);
        }


        public async Task<int> CreateUserAsync(UserModel user, CancellationToken token = default)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync(token);

            return user.Id;
        }

        public async Task<bool> UpdateUserAsync(UserModel user, CancellationToken token = default)
        {
            _context.Users.Update(user);

            await _context.SaveChangesAsync(token); 
            
            return true;
        }

        public async Task<bool> DeleteUserAsync(UserModel user, CancellationToken token = default)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync(token);

            return true;
        }

        public async Task<List<UserModel>> GetUsersAsync(UserQueryOptions queryOptions, CancellationToken token = default)
        {
            var query = _context.Users
                .Where(u => string.IsNullOrEmpty(queryOptions.Search) || u.FullName.Contains(queryOptions.Search))
                .OrderByDescending(u => u.Id)
                .Skip((int)queryOptions.Skip!)
                .Take((int)queryOptions.ItemsOfPage!)
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    CreatedDate = u.CreatedDate,
                    Role = u.Role
                });

                return await query.ToListAsync(token);
        }

        public async Task<int> GetCountUsersCountAsync(UserQueryOptions queryOptions, CancellationToken token = default)
        {
            return await _context.Users
                   .Where(u => string.IsNullOrEmpty(queryOptions.Search) || u.FullName.Contains(queryOptions.Search))
                   .CountAsync(token);
        }

    }
}
