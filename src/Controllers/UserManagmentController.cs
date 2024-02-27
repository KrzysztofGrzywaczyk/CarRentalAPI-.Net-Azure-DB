using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "administrator")]
public class UserManagmentController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public ActionResult CreateUser([FromBody] CreateUserDto userDto)
    {
        var path = userService.AddUser(userDto);

        return Ok(path);

    }

    [HttpDelete("{userId}")]
    public ActionResult DeleteUser(int userId)
    {
        userService.DeleteUser(userId);

        return NoContent();
    }

    [HttpGet("all")]
    public ActionResult GetAllUsers([FromQuery] UserQuery query)
    {
        return Ok(userService.GetAllUsers(query));
    }

    [HttpGet("{userId}")]
    public ActionResult GetUserById([FromRoute] int userId)
    {
        return Ok(userService.GetUserById(userId));
    }

    [HttpPut("{userId}")]
    public ActionResult PutUser([FromBody] UpdateUserDto userDto, [FromRoute] int userId)
    {
        userService.PutUser(userDto, userId);
        return Ok();
    }
}
