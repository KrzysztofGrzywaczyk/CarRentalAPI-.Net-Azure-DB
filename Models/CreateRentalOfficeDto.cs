using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class CreateRentalOfficeDto
    {
        [Required]
        [MaxLength(25)]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool AcceptUnder23 { get; set; }
        public string? ConntactEmail { get; set; }
        public string? ConntactNumber { get; set; }
        [Required]
        [MaxLength(30)]
        public string? City { get; set; }
        [Required]
        [MaxLength(30)]
        public string? Street { get; set; }
        public string? PostalCode { get; set; }

    }
}
