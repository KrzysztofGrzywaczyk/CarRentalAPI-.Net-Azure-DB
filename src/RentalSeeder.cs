using CarRentalAPI.Entities;

namespace CarRentalAPI
{
    public class RentalSeeder
    {
        private readonly RentalDbContext _dbContext;

        private readonly ILogger<RentalSeeder> _logger;
        public RentalSeeder(RentalDbContext dbContext, ILogger<RentalSeeder> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public void SeedBasicRoles()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.roles.Any())
                {
                    _logger.LogInformation("Seeder seed the basic roles in the database ...");

                    var roles = new List<Role>()
                    {
                        new Role() {Name = "administrator"},
                        new Role() {Name = "manager" },
                        new Role() {Name = "employee"},
                        new Role() {Name = "user"}
                    };

                    _dbContext.roles.AddRange(roles);
                    _dbContext.SaveChanges();

                    _logger.LogInformation("Basic roles successfully CREATED");
                }
            }
        }

        public void SeedSampleCars()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.rentalOffices.Any())
                {
                    _logger.LogInformation("Seeder seed the few sample records in the database ...");

                    var rentals = GetRentalOffices();
                    _dbContext.rentalOffices.AddRange(rentals);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<RentalOffice> GetRentalOffices()
        {
            var rentals = new List<RentalOffice>()
            {
                new RentalOffice()
                {
                    Name = "Example Car Rental",
                    Description = "It is short description of what kind of Car Rental Office is it",
                    Category = RentalOffice.RentalCategory.Luxury,
                    AcceptUnder23 = false,
                    ConntactEmail = "luxury@example.com",
                    ConntactNumber = "0123456789",
                    Address = new Address()
                    {
                        City = "Wroclaw",
                        Street = "Example Street 10",
                        PostalCode = "50-420"
                    },
                    Cars = new List<Car>()
                    {
                        new Car()
                        {
                            PlateNumber = "DW 3HL50",
                            Brand = "Mercedes",
                            Model = "S 600 Long",
                            Year = 2021,
                            Segment = 'F'
                        },

                        new Car()
                        {
                            PlateNumber = "DX 12E43",
                            Brand = "Cadillac",
                            Model = "XTS",
                            Year = 2020,
                            Segment = 'F'
                        },
                    },
                }
            };

            _logger.LogInformation("Sample records successfully created in database");

            return rentals;
        }

    }

}