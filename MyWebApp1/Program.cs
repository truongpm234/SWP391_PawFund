using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.Configuration;
using MyWebApp1.Services;
using MyWebApp1.Extensions;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ cho Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddCustomServices(builder.Configuration); // Thêm tất cả dịch vụ từ lớp mở rộng

// Cấu hình Google authentication
builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection("Google"));

// Thêm DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

// Xây dựng ứng dụng
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pawfund Platform v1"));
//}zz

// Cấu hình middleware cho Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pawfund Platform V1");
        c.RoutePrefix = "swagger"; // Đặt đường dẫn đến Swagger
    });
}

// Kích hoạt Routing
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Chạy ứng dụng
app.Run();
