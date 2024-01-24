using CarRentalAPI.Handlers;
using Microsoft.AspNetCore.Mvc;
using CarRentalAPI.Entities;

namespace CarRentalAPI.Controllers;

//to delete

[ApiController]
[Route("roles")]
public class RoleController : ControllerBase
{
    private readonly ILogHandler _logHandler;
    private readonly RentalDbContext _dbContext;
    private const string entityType = "Car";

    public RoleController(ILogHandler logHandler, RentalDbContext dbContext)
    {
        _logHandler = logHandler;
        _dbContext = dbContext;
    }
    [HttpGet]
    public ActionResult GetAllRoles()
    {
        _logHandler.LogNewRequest(entityType, ILogHandler.RequestEnum.GET);

         var roles = _dbContext.roles
                .ToList();

        return Ok(roles);
       
    }
}