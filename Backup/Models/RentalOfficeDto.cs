namespace CarRentalAPI.Models
{
    public class RentalOfficeDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Category { get; set; }

        public bool AcceptUnder23 { get; set; }

        public string? City { get; set;}

        public string? Street { get; set; }

        public string? PostalCode { get; set;}

        public List<PresentCarDto>? Cars { get; set;}
    }
}
