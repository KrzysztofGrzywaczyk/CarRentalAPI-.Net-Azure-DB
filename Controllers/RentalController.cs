using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Services;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace CarRentalAPI.Controllers
{
    [Route("api/rentaloffices")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly RentalDbContext dbContext;

        private readonly IMapper mapper;

        private readonly IRentalService rentalService;
        

        public RentalController(RentalDbContext dbContext, IMapper mapper, IRentalService rentalService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.rentalService = rentalService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RentalOfficeDto>> GetAll()
        {
            return Ok(rentalService.GetRentalAll());
        }

        [HttpGet("{id}")]
        public ActionResult<RentalOfficeDto> Get([FromRoute] int id) 
        {
            var rentalDto = rentalService.GetRentalById(id);

            if (rentalDto != null)
            {
                return Ok(rentalDto);
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public ActionResult<RentalOfficeDto> Get([FromBody] RentalOfficeUpdateDto dto, [FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rentalService.PutRentalById(dto, id);
            return Ok();
        }

        [HttpPost]
        public ActionResult CreateRentalOffice([FromBody] CreateRentalOfficeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var path = rentalService.CreateRental(dto);

            return Created(path, null);
        }

        [HttpDelete("{id}")]

        public ActionResult Delete([FromRoute] int id)
        {
                        
            rentalService.DeleteRental(id);
            return NoContent();
        }
    }
}
