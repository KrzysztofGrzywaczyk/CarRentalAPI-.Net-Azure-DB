using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class RentalOfficeUpdateDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool AcceptUnder23 { get; set; }
        public string ConntactEmail { get; set; }
        public string ConntactNumber { get; set; }
    }
}
