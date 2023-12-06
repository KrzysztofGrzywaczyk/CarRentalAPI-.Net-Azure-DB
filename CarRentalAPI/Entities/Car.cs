using System.Security.Cryptography.X509Certificates;

namespace CarRentalAPI.Entities
{
    public class Car
    {
        public enum FuelType 
        {
            Gas,
            Oil,
            LPG,
            GasHybrid
        }

        public int Id { get; set; }
        public string? PlateNumber { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public FuelType Fuel { get; set; }
        public char Segment { get; set; }
        public int RentalOfficeId { get; set; }
        public virtual RentalOffice? RentalOffice { get; set; }

    }
}
