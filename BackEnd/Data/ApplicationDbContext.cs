using BackEnd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Data
{


    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    { 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=DESKTOP-8DKQPFJ\\SQLEXPRESS;Database=Db;User ID=sa;Password=admin@123;Trusted_Connection=False;");
        //    base.OnConfiguring(optionsBuilder);
        //}
        public DbSet<Login> Logins { get; set; } = null!;
        public DbSet<Register> Registers { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure this line is present

            // Your custom configurations go here
            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasIndex(e => e.UId, "UId")
                   .IsUnique();
                   
            });

            // Other entity configurations...
        }
    }
}