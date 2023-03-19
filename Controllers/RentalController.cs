using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Reflection.Metadata.Ecma335;

namespace CarRentalAPI.Controllers
{
    [Route("api/rentaloffices")]
    public class RentalController : ControllerBase
    {
        private readonly RentalDbContext _dbContext;

        private readonly IMapper _mapper;

        public RentalController(RentalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RentalOfficeDto>> GetAll()
        {
            var rentals = _dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .ToList();

            var rentalsDto = _mapper.Map<List<RentalOfficeDto>>(rentals);

            return Ok(rentalsDto);
        }

        [HttpGet("{id}")]
        public ActionResult<RentalOfficeDto> Get([FromRoute] int id) 
        {
            var rental = _dbContext.rentalOffices
                .Include(r => r.Address)
                .Include(r => r.Cars)
                .FirstOrDefault(r => r.Id == id);

            

            if (rental == null)
            {
                return NotFound();
            }
            else
            {
                var rentalDto = _mapper.Map<RentalOfficeDto>(rental);
                return Ok(rentalDto);
            }
        }

        [HttpPost]
        public ActionResult CreateRentalOffice([FromBody] CreateRentalOfficeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rentalOffice = _mapper.Map<RentalOffice>(dto);
            _dbContext.rentalOffices.Add(rentalOffice);
            _dbContext.SaveChanges();

            return Created($"/api/rentaloffices/{rentalOffice.Id}", null);
        }
      
    }
}
