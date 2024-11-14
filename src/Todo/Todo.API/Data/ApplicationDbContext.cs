using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Todo.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options) { }

        public DbSet<TaskModel> Tasks => Set<TaskModel>();
        public DbSet<UserModel> Users => Set<UserModel>();
        public DbSet<RefreshTokenModel> RefreshTokens => Set<RefreshTokenModel>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}
