using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
using MyWebApp1.Configuration;
using MyWebApp1.Services;
using MyWebApp1.Extensions;
using Microsoft.AspNetCore.Authentication.Google;
using MyWebApp1.DTO;
using Microsoft.Extensions.Options;
using MyWebApp1.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder.AllowAnyHeader()
                     .AllowAnyMethod()
                     .WithOrigins("http://localhost:5111", "http://localhost:5173");
    });
});


// swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen();
<<<<<<< HEAD
builder.Services.AddScoped<TransactionService>();
builder.Services.AddCustomServices(builder.Configuration); // Thêm tất cả dịch vụ từ lớp mở rộng
=======

builder.Services.AddCustomServices(builder.Configuration);
>>>>>>> Dev-for-test

// Cấu hình Google
builder.Services.Configure<GoogleOptions>(builder.Configuration.GetSection("Google"));

// Thêm DbContext
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<TransactionService>();

builder.Services.AddScoped<IShelterService, ShelterService>();


// build app
var app = builder.Build();

<<<<<<< HEAD
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pawfund Platform v1"));
}

//if (app.Environment.IsDevelopment())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pawfund Platform v1"));
//}zz

// Cấu hình middleware cho Swagger
=======
>>>>>>> Dev-for-test
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

app.UseCors("CorsPolicy");

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();
