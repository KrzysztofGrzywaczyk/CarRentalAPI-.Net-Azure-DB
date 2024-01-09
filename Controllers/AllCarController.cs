using CarRentalAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [ApiController]
    [Route("all/cars")]
    public class AllCarController : ControllerBase
    {
        private ICarService _carService;
        public AllCarController(ICarService service) 
        {
            _carService = service;
        }

        [HttpGet]
        public ActionResult GetAllCarsInBase()
        {
            return Ok(_carService.GetAllCarsInBase());
        }
    }
}
