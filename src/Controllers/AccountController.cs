using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[Route("api/account")]
[ApiController]
[AllowAnonymous]
public class AccountController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public ActionResult RegisterUser([FromBody]CreateUserDto userDto)
    {
        userService.AddUser(userDto);
        return Ok();
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody] LoginDto loginDto)
    {
        string token = userService.GenerateToken(loginDto);
        return Ok(token);
    }
}
