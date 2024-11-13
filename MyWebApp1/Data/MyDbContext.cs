using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using MyWebApp1.Models;
using MyWebApp1.Models.MyWebApp1.Models;
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
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<PetCategory> PetCategories { get; set; }
        public DbSet<Adoption> Adoptions { get; set; }
        public DbSet<PetImage> PetImages { get; set; }
        public DbSet<DonationEvent> DonationEvents { get; set; }
        public DbSet<DonationImage> DonationImages { get; set; }
        public DbSet<MyWebApp1.Models.Transaction> Transactions { get; set; }
        public DbSet<MyWebApp1.Models.TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<UserApprove> UserApproves { get; set; }
        public DbSet<AdoptionConfirmation> AdoptionConfirmations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<User>()
                .HasOne(u => u.Shelter) // 1 user - 1 shelter
                .WithOne(s => s.User)         
                .HasForeignKey<User>(u => u.ShelterId);

            // Các thiết lập khác...
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<PetCategory>().ToTable("PetCategory");
            modelBuilder.Entity<MyWebApp1.Models.TransactionStatus>().ToTable("TransactionStatus");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Pet>().ToTable("Pet");
            modelBuilder.Entity<Shelter>().ToTable("Shelter");
            modelBuilder.Entity<UserRole>().ToTable("UserRole");
            modelBuilder.Entity<Adoption>().ToTable("Adoption");
            modelBuilder.Entity<PetImage>().ToTable("PetImage");
            modelBuilder.Entity<DonationEvent>().ToTable("DonationEvent");
            modelBuilder.Entity<DonationImage>().ToTable("DonationImage");
            modelBuilder.Entity<TransactionType>().ToTable("TransactionType");
            modelBuilder.Entity<MyWebApp1.Models.Transaction>().ToTable("Transaction");

            modelBuilder.Entity<MyWebApp1.Models.Transaction>()
                .Property(t => t.TransactionAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Pet>()
                .HasOne(p => p.PetCategory)
                .WithMany()
                .HasForeignKey(p => p.PetCategoryId)
                .IsRequired(false);
            modelBuilder.Entity<Pet>()
                .HasMany(p => p.PetImages) 
                .WithOne(pi => pi.Pet)  
                .HasForeignKey(pi => pi.PetId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<AdoptionConfirmation>()
        .HasKey(ac => ac.ConfirmationId);

            modelBuilder.Entity<AdoptionConfirmation>()
                .ToTable("AdoptionConfirmation");

        }
    }
}