using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;


namespace CarRentalAPI.Controllers
{
    [Route("api/rentaloffices")]
    public class RentalController : ControllerBase
    {
        private readonly RentalDbContext dbContext;

        private readonly IMapper mapper;

        private readonly IDeleteRentalHandler deleteRentalHandler;

        private readonly IGetRentalHandler getRentalHandler;

        private readonly IPostRentalHandler postRentalHandler;

        private readonly IPutRentalHandler putRentalHandler;
        

        public RentalController(RentalDbContext dbContext, IMapper mapper, IDeleteRentalHandler deleteRentalHandler , IGetRentalHandler getRentalHandler, IPutRentalHandler putRentalHandler, IPostRentalHandler postRentalHandler)
        {
            this.dbContext = dbContext;
            this.deleteRentalHandler = deleteRentalHandler;
            this.getRentalHandler = getRentalHandler;
            this.mapper = mapper;
            this.postRentalHandler = postRentalHandler;
            this.putRentalHandler = putRentalHandler;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RentalOfficeDto>> GetAll()
        {
            return Ok(getRentalHandler.HandleGetAllRequest());
        }

        [HttpGet("{id}")]
        public ActionResult<RentalOfficeDto> Get([FromRoute] int id) 
        {
            var rentalDto = getRentalHandler.HandleGetByIdRequest(id);

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

            bool success = putRentalHandler.HandlePutById(dto, id);
            return success ? Ok() : BadRequest(); 
        }

        [HttpPost]
        public ActionResult CreateRentalOffice([FromBody] CreateRentalOfficeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var path = postRentalHandler.HandlePostRental(dto);

            return Created(path, null);
        }

        [HttpDelete("{id}")]

        public ActionResult Delete([FromRoute] int id)
        {
                        
            bool success = deleteRentalHandler.HandleDeleteRental(id);
            return success ? NoContent() : NotFound();
        }
    }
}
