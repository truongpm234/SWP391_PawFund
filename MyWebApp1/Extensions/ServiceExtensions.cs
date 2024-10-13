using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MyWebApp1.Services;
using MyWebApp1.Configuration;
using MyWebApp1.Data;

namespace MyWebApp1.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<PetService>();

            // Gọi phương thức AddSwaggerAuthentication từ DependencyInjection
            services.AddSwaggerAuthentication();
            services.AddScoped<UserService>();

            services.AddLogging(config =>
            {
                config.AddConsole(); // Thêm ghi log vào console
                config.AddDebug(); // Thêm ghi log vào debug
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            })
            .AddGoogle(googleOptions =>
            {
                IConfigurationSection googleAuthNSection = configuration.GetSection("Authentication:Google");
                googleOptions.ClientId = googleAuthNSection["ClientId"];
                googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
            });

            services.AddDbContext<MyDbContext>(e => e.UseSqlServer(configuration.GetConnectionString("DBCS")));
        }
    }
}
