using AutoMapper;
using CarRentalAPI.Entities;
using CarRentalAPI.Services;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace CarRentalAPI.Controllers;

[Route("api/rentaloffices")]
[ApiController]
[Authorize(Roles = "administrator,rentalOwner")]
public class RentalsController : ControllerBase
{
    private readonly IRentalService _rentalService;
    
    public RentalsController(RentalDbContext dbContext, IMapper mapper, IRentalService rentalService)
    {
        _rentalService = rentalService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PresentRentalOfficeDto>> GetAll()
    {
        return Ok(_rentalService.GetRentalAll());
    }

    [HttpGet("{id}")]
    public ActionResult<PresentRentalOfficeDto> Get([FromRoute] int id) 
    {
        var rentalDto = _rentalService.GetRentalById(id);

        if (rentalDto != null)
        {
            return Ok(rentalDto);
        }
        return NotFound();
    }

    [HttpPut("{id}")]
    public ActionResult<PresentRentalOfficeDto> Get([FromBody] UpdateRentalOfficeDto dto, [FromRoute] int id)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _rentalService.PutRentalById(dto, id);
        return Ok();
    }

    [HttpPost]
    public ActionResult CreateRentalOffice([FromBody] CreateRentalOfficeDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

        var path = _rentalService.CreateRental(dto);

        return Created(path, null);
    }

    [HttpDelete("{id}")]

    public ActionResult Delete([FromRoute] int id)
    {
        _rentalService.DeleteRental(id);
        return NoContent();
    }
}
