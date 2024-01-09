using CarRentalAPI.Models;

namespace CarRentalAPI.Services
{
    public interface IUserService
    {
        public void AddUser(AddUserDto userDto);

        public string GenerateToken (LoginDto loginDto);
    }
}
