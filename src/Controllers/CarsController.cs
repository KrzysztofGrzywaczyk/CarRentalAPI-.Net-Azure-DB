using CarRentalAPI.Models;
using CarRentalAPI.Models.Queries;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers;

[ApiController]
[Route("api/rentals/{rentalId}/cars")]
[Authorize(Roles = "administrator,rentalOwner,employee")]
public class CarsController(ICarService carService) : ControllerBase
{
    [HttpGet]
    public ActionResult GetAllCarsInRental([FromRoute] int rentalId, [FromQuery] CarQuery query)
    {
        return Ok( carService.GetAllCarsInRental(rentalId, query));
    }

    [HttpGet("{carId}")]
    public ActionResult GetCar([FromRoute] int rentalId, [FromRoute] int carId)
    {
        return Ok(carService.GetCarById(rentalId, carId));
    }

    [HttpPost]
    public ActionResult CreateCar([FromRoute] int rentalId, [FromBody] CreateUpdateCarDto dto)
    {
        var path = carService.CreateCar(rentalId, dto);
        return Created(path, null);
    }

    [HttpDelete("{carId}")]
    public ActionResult DeleteCar([FromRoute] int rentalId, [FromRoute] int carId)
    {
        carService.DeleteCar(rentalId, carId);
        return NoContent();
    }

    [HttpPut("{carId}")]
    public ActionResult PutCar([FromRoute] int rentalId, [FromRoute] int carId, [FromBody] CreateUpdateCarDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var path = carService.PutCar(rentalId, carId, dto);

        return Ok(path);
    }  
}
