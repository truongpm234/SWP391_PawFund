using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using MyWebApp1.Services;
using MyWebApp1.Configuration;
using MyWebApp1.Data;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using MyWebApp1.Controllers;

namespace MyWebApp1.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddHttpContextAccessor();
        
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddSwaggerAuthentication();

            services.AddScoped<AdoptionService>();

            services.AddScoped<UserService>();

            services.AddScoped<ManagerService>();

            services.AddScoped<AdminService>();

            services.AddScoped<StaffService>();

            services.AddScoped<DonationEventService>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IShelterService, ShelterService>();

            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            //});

            services.AddLogging(config =>
            {
                config.AddConsole();
                config.AddDebug();
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role", "Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireClaim("Role", "User"));
                options.AddPolicy("ManagerOnly", policy => policy.RequireClaim("Role", "Manager"));
                options.AddPolicy("StaffOnly", policy => policy.RequireClaim("Role", "Staff"));
                options.AddPolicy("BlockOnly", policy => policy.RequireClaim("Role", "Block"));
                options.AddPolicy("UserOrStaff", policy => policy.RequireClaim("Role", "User", "Staff"));
                options.AddPolicy("ManagerOrStaff", policy => policy.RequireClaim("Role", "Manager", "Staff"));
                options.AddPolicy("ManagerOrAdmin", policy => policy.RequireClaim("Role", "Manager", "Admin"));

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
                    ValidateLifetime = false,
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
            services.AddControllers();
        }
    }
}
