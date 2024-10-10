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

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite key

            modelBuilder.Entity<MyWebApp1.Models.Transaction>()
                .Property(t => t.TransactionAmount)
                .HasColumnType("decimal(18,2)"); // 18 là tổng số chữ số, 2 là số chữ số sau dấu thập phân
            
            modelBuilder.Entity<Pet>()
               .HasOne(p => p.PetCategory)
               .WithMany()
               .HasForeignKey(p => p.PetCategoryId);

            modelBuilder.Entity<User>().ToTable("User");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder
            .UseMySql("Server=DESKTOP-RACPEP4\\SQLEXPRESS;Database=PawFund;User Id=sa;Password=123456;", 
            new MySqlServerVersion(new Version(8, 0, 21))) // Ensure you specify your MySQL version
            .LogTo(Console.WriteLine, LogLevel.Information);
    }
}


    }
}