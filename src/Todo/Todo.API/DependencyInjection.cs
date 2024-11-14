using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Todo.API
{
    public static class DependencyInjection
    {
        public static void AddApiVersionForSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1.0);
                x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;

                x.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(), // Enables version in URL
                    new QueryStringApiVersionReader("v"), // Enables version in query string, e.g., ?v=1.0
                    new HeaderApiVersionReader() // Enables version in headers
                );

            }).AddMvc().AddApiExplorer();
        }

        public static void AddJwtConfiguration(this IServiceCollection services, ConfigurationManager config)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = config["JwtSettings:Issuer"],
                    ValidAudience = config["JwtSettings:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                };
            });
        }
    }
}
