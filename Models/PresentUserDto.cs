using CarRentalAPI.Entities;

namespace CarRentalAPI.Models
{
    public class PresentUserDto
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? Nickname { get; set; }

        public string? RoleName { get; set; }
    }
}
