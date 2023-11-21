using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class CreateCarDto
    {
        [Required]
        [MaxLength(8)]
        public string? PlateNumber { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Brand { get; set; }
        [Required]
        [MaxLength(20)]
        public string? Model { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        [MaxLength(2)]
        public string? Segment { get; set; }
    }
}
