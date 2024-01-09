using CarRentalAPI.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Entities
{
    public class RentalDbContext : DbContext

    {

        private string? _ConnectionString { get; set; }
        public DbSet<RentalOffice> rentalOffices { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<Car> cars { get; set; }

        public DbSet<User> users { get; set; }
        public DbSet<Role> roles { get; set; }

        public RentalDbContext(RentalDbContextConfiguration config)
        {
            _ConnectionString = config.DatabaseConnectionString;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RentalOffice>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(30);
            modelBuilder.Entity<Car>()
                .Property(c => c.PlateNumber)
                .IsRequired();
            modelBuilder.Entity<Address>()
                .Property(a => a.City)
                .IsRequired()
                .HasMaxLength(30);
            modelBuilder.Entity<Address>()
                .Property(a => a.Street)
                .IsRequired()
                .HasMaxLength(45);
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();
            modelBuilder.Entity<Role>()
                .Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(30);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_ConnectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
        }
    }
}
