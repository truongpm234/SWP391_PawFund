using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyWebApp1.Models;
using MyWebApp1.Configuration;
using MyWebApp1.Services;
using MyWebApp1.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCustomServices(builder.Configuration); // Thêm tất cả dịch vụ từ lớp mở rộng

builder.Services.AddDbContext<MyDbContext>(e => e.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pawfund Platform v1"));
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Đặt trước app.UseAuthorization()
app.UseAuthorization();

app.MapControllers();

app.Run();
