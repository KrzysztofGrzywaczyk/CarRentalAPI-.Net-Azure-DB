using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class RentalOfficeUpdateDto
    {
        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }
        [MaxLength(75)]
        public string? Description { get; set; }
        [Required]
        [MaxLength(15)]
        public string? Category { get; set; }
        [Required]
        public bool AcceptUnder23 { get; set; }
        [EmailAddress]
        public string? ConntactEmail { get; set; }
        [Phone]
        public string? ConntactNumber { get; set; }
    }
}
