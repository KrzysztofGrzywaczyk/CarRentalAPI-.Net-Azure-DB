using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[ApiController]
[Route("api/rentals/{rentalId}/cars")]
[Authorize(Roles = "admin,manager,employee")]
public class CarsController : ControllerBase
{
    public readonly ICarService _carService;
    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpGet]
    public ActionResult GetAllCarsInRental([FromRoute] int rentalId)
    {
        return Ok( _carService.GetAllCarsInRental(rentalId));
    }

    [HttpGet("{carId}")]
    public ActionResult GetCar([FromRoute] int rentalId, [FromRoute] int carId)
    {
        return Ok(_carService.GetCarById(rentalId, carId));
    }

    [HttpPost]
    public ActionResult CreateCar([FromRoute] int rentalId, [FromBody] CreateCarDto dto)
    {
        var path = _carService.CreateCar(rentalId, dto);
        return Created(path, null);
    }

    [HttpDelete("{carId}")]
    public ActionResult DeleteCar([FromRoute] int rentalId, [FromRoute] int carId)
    {
        _carService.DeleteCar(rentalId, carId);
        return NoContent();
    }

    [HttpPut("{carId}")]
    public ActionResult PutCar([FromRoute] int rentalId, [FromRoute] int carId, [FromBody] CreateCarDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var path = _carService.PutCar(rentalId, carId, dto);

        return Ok(path);
    }  
}
