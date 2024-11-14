using System.Text.Json;

namespace Todo.API.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                context.Database.MigrateAsync().GetAwaiter().GetResult();
                await SeedAsync(context);
            }          
        }

        private static async Task SeedAsync(ApplicationDbContext context)
        {
           await SeedUserAsync(context);
        }

        private static async Task SeedUserAsync(ApplicationDbContext context)
        {
            if (!await context.Users.AnyAsync())
            {
                var user = InitialData.GetUsers();
                Console.WriteLine("USer for init " + JsonSerializer.Serialize(user));

                await context.Users.AddRangeAsync(InitialData.GetUsers());
                await context.SaveChangesAsync();
            }
        }
    }
}
