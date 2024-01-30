using CarRentalAPI.Handlers;
using Microsoft.AspNetCore.Mvc;
using CarRentalAPI.Entities;

namespace CarRentalAPI.Controllers;

//to delete

[ApiController]
[Route("roles")]
public class RoleController(RentalDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public ActionResult GetAllRoles()
    {

         var roles = dbContext.roles
                .ToList();

        return Ok(roles);
       
    }
}