using Microsoft.EntityFrameworkCore;
using MyWebApp1.Models;

namespace MyWebApp1.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
