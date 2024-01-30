using CarRentalAPI.Models.Queries;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[ApiController]
[Route("all/cars")]
[AllowAnonymous]
public class AllCarController : ControllerBase
{
    private ICarService _carService;
    public AllCarController(ICarService service) 
    {
        _carService = service;
    }

    [HttpGet]
    public ActionResult GetAllCarsInBase([FromQuery] CarQuery query)
    {
        return Ok(_carService.GetAllCarsInBase(query));
    }
}
