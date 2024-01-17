using System.ComponentModel.DataAnnotations;
using static CarRentalAPI.Entities.RentalOffice;

namespace CarRentalAPI.Models
{
    public class UpdateRentalOfficeDto
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        [MaxLength(15)]
        public string? Category { get; set; }

        [Required]
        public bool AcceptUnder23 { get; set; }

        [EmailAddress]
        public string? ConntactEmail { get; set; }

        [Phone]
        public string? ConntactNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public string? City { get; set; }

        [Required]
        [MaxLength(45)]
        public string? Street { get; set; }

        [MaxLength(6)]
        [RegularExpression("^[0-9]{2}-[0-9]{3}$", ErrorMessage = "You need to enter postal code in proper polish format 00-000")]
        public string? PostalCode { get; set; }

    }
}
