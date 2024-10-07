using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using MyWebApp1.Entities;
using MyWebApp1.Models;
using MyWebApp1.Models.MyWebApp1.Entities;
using System.Data;
using System.Transactions;

namespace MyWebApp1.Data
{
    public class MyDbContext : DbContext
    {
        private readonly ILogger<MyDbContext> _logger;

        public MyDbContext(DbContextOptions<MyDbContext> options, ILogger<MyDbContext> logger)
            : base(options)
        {
            _logger = logger;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<PetCategory> PetCategories { get; set; }
        public DbSet<Adoption> Adoptions { get; set; }
        public DbSet<PetImage> PetImages { get; set; }
        public DbSet<DonationEvent> DonationEvents { get; set; }
        public DbSet<DonationImage> DonationImages { get; set; }
        public DbSet<MyWebApp1.Models.Transaction> Transactions { get; set; }
        public DbSet<MyWebApp1.Models.TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Định nghĩa khóa chính tổng hợp cho UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite key

            // Định nghĩa kiểu dữ liệu cho TransactionAmount
            modelBuilder.Entity<MyWebApp1.Models.Transaction>()
                .Property(t => t.TransactionAmount)
                .HasColumnType("decimal(18,2)"); // 18 là tổng số chữ số, 2 là số chữ số sau dấu thập phân

            // Định nghĩa tên bảng cho User
            modelBuilder.Entity<User>().ToTable("User");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Data Source=DESKTOP-1T8C15J\\SQLEXPRESS; Initial Catalog=PawFund; User ID=sa; Password=123456;TrustServerCertificate=True;")
                    .LogTo(Console.WriteLine, LogLevel.Information); // Ghi log ra console
            }
        }

    }
}