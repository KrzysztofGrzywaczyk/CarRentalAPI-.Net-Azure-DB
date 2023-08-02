using CarRentalAPI.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Entities
{
    public class RentalDbContext : DbContext

    {

        private string? ConnectionString { get; set; }
        public DbSet<RentalOffice> rentalOffices { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<Car> cars { get; set; }

        public RentalDbContext(RentalDbContextConfiguration config)
        {
            this.ConnectionString = config.DatabaseConnectionString;
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
                .HasMaxLength (30);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.ConnectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
        }
    }
}
