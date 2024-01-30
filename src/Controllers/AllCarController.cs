using CarRentalAPI.Models.Queries;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[ApiController]
[Route("all/cars")]
[AllowAnonymous]
public class AllCarController(ICarService carService) : ControllerBase
{

    [HttpGet]
    public ActionResult GetAllCarsInBase([FromQuery] CarQuery query)
    {
        return Ok(carService.GetAllCarsInBase(query));
    }
}
