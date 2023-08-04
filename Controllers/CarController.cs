using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [Route("api/rentals/{rentalId}/cars")]
    [ApiController]
    public class CarController : ControllerBase
    {
        public readonly ICarService carService;
        public CarController(ICarService carService)
        {
            this.carService = carService;
        }

        [HttpGet]
        public ActionResult GetAllCars([FromRoute] int rentalId)
        {
            return Ok();
        }

        [HttpGet("{id}")]
        public ActionResult GetCar([FromRoute] int rentalId, [FromRoute] int id) 
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult CreateCar([FromRoute] int rentalId, [FromBody] CreateCarDto dto)
        {
            var path = carService.CreateCar(rentalId, dto);
            return Created("path", null);
        }
        
    }
}
