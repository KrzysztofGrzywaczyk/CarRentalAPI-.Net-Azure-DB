namespace CarRentalAPI.Models
{
    public class CarDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public char Segment { get; set; }
    }
}
