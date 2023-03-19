using Microsoft.EntityFrameworkCore;

namespace CarRentalAPI.Entities
{
    public class RentalDbContext : DbContext

    {
        private string _connectionString =
            "Server=tcp:serv-sql-kgrz.database.windows.net,1433;Initial Catalog = CarRentalDB; Persist Security Info=False;User ID = myadmin; Password=CarRentalAPI1; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;";
        public DbSet<RentalOffice> rentalOffices { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<Car> cars { get; set; }

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
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
