using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;

namespace CarRentalAPI.Services;

public interface IUserService
{
    public string AddUser (CreateUserDto userDto);

    public void DeleteUser(int userId);

    public string GenerateToken (LoginDto loginDto);

    public PagedResult<PresentUserDto> GetAllUsers(UserQuery query);

    public PresentUserDto GetUserById(int userId);

    public void PutUser (UpdateUserDto userDto, int userId);
}
