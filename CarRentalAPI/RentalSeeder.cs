using CarRentalAPI.Entities;

namespace CarRentalAPI
{
    public class RentalSeeder
    {
        private readonly RentalDbContext _dbContext;
        public RentalSeeder(RentalDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed(RentalDbContext dbContext)
        {
            if (_dbContext.Database.CanConnect())
            {
                if (!_dbContext.rentalOffices.Any())
                {
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
                    Name = "Example One Car Rental",
                    Description = "It is short description of what kind of Car Rental Office is it",
                    Category = RentalOffice.RentalCategory.Luxury,
                    AcceptUnder23 = false,
                    ConntactEmail = "luxury@example.com",
                    ConntactNumber = "0123456789",
                    Address = new Address()
                    {
                        City = "Wroclaw",
                        Street = "Example Street 10",
                        PostalCode = "50-419"
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

            return rentals;
        }

    }

}