using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.Configuration;
using MyWebApp1.Services;
using MyWebApp1.Extensions;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCustomServices(builder.Configuration);

// Cấu hình Google
builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection("Google"));

// Thêm DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

// build app
var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pawfund Platform v1"));
//}

// Cấu hình middleware cho Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pawfund Platform V1");
        c.RoutePrefix = "swagger";
    });
}

// Kích hoạt Routing
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
