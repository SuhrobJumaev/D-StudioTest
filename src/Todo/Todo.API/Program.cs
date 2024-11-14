
using log4net.Config;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Todo.API;
using Todo.API.Helpers;
using Todo.API.Intefaces;
using Todo.API.MIddlewares;
using Todo.API.Models.SettingModels;
using Todo.API.Respositories;
using Todo.API.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var config = builder.Configuration;

builder.Services.Configure<JwtSettingsModel>(config.GetSection("JwtSettings"));

//Other
builder.Services.AddJwtConfiguration(config);
builder.Services.AddApiVersionForSwagger();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

//All Services
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService,TaskService>();

//All Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DataBase")));

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(x => x.OperationFilter<SwaggerDefaultValues>());

var app = builder.Build();

XmlConfigurator.Configure(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    foreach (var description in app.DescribeApiVersions())
    {
        x.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
            description.GroupName);

    }
});

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

await app.InitializeDatabaseAsync();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
