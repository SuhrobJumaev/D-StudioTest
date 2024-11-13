namespace Todo.API.Data.Extensions
{
    public static class DatabaseExtensions
    {
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.MigrateAsync().GetAwaiter().GetResult();
        }
    }
}
