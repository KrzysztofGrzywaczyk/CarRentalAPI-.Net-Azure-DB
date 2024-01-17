using CarRentalAPI.Models;

namespace CarRentalAPI.Services;

public interface IUserService
{
    public string AddUser (CreateUserDto userDto);

    public void DeleteUser(int userId);

    public string GenerateToken (LoginDto loginDto);

    public List<PresentUserDto> GetAllUsers();

    public PresentUserDto GetUserById(int userId);

    public void PutUser (UpdateUserDto userDto, int userId);
}
