using Microsoft.EntityFrameworkCore;
using MyWebApp1.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDbContext>(e => e.UseSqlServer(builder.Configuration.GetConnectionString("DBCS")));
///Unable to create a 'DbContext' of type ''. 
///The exception 'Unable to resolve service for type 'Microsoft.EntityFrameworkCore.DbContextOptions`1[MyWebApp1.Models.MyDbContext]' 
///while attempting to activate 'MyWebApp1.Models.MyDbContext'.' was thrown while attempting to create an instance. 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
