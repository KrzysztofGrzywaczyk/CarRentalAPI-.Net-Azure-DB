using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[Route("api/account")]
[ApiController]
[AllowAnonymous]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public ActionResult RegisterUser([FromBody]CreateUserDto userDto)
    {
        _userService.AddUser(userDto);
        return Ok();
    }

    [HttpPost("login")]
    public ActionResult Login([FromBody] LoginDto loginDto)
    {
        string token = _userService.GenerateToken(loginDto);
        return Ok(token);
    }
}
