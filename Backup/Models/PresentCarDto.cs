using static CarRentalAPI.Entities.Car;

namespace CarRentalAPI.Models
{
    public class PresentCarDto
    {
        public int Id { get; set; }

        public string? Brand { get; set; }

        public string? Model { get; set; }

        public string? Fuel { get; set; }

        public int Year { get; set; }

        public char Segment { get; set; }
    }
}
