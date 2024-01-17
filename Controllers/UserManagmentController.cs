using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "administrator")]
public class UserManagmentController : ControllerBase
{
    private readonly IUserService _userService;

    public UserManagmentController( IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public ActionResult CreateUser([FromBody] CreateUserDto userDto)
    {
        var path = _userService.AddUser(userDto);

        return Ok(path);

    }

    [HttpDelete("{userId}")]
    public ActionResult DeleteUser(int userId)
    {
        _userService.DeleteUser(userId);

        return NoContent();
    }

    [HttpGet("all")]
    public ActionResult GetAllUsers()
    {
        return Ok(_userService.GetAllUsers());
    }

    [HttpGet("{userId}")]
    public ActionResult GetUserById([FromRoute] int userId)
    {
        return Ok(_userService.GetUserById(userId));
    }

    [HttpPut("{userId}")]
    public ActionResult PutUser([FromBody] UpdateUserDto userDto, [FromRoute] int userId)
    {
        _userService.PutUser(userDto, userId);
        return Ok();
    }
}
