namespace CarRentalAPI.Entities
{
    public class RentalOffice
    {
        public enum RentalCategory
        {
            Casual,
            Sport,
            Luxury
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public RentalCategory Category { get; set; } = RentalCategory.Casual;
        public bool AcceptUnder23 { get; set; }
        public string? ConntactEmail { get; set; }
        public string? ConntactNumber { get; set; }
        public int AddressID { get; set; }
        public virtual Address? Address { get; set; }
        public virtual List<Car>? Cars { get; set; }
     
    }
}
