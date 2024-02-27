using CarRentalAPI.Services;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CarRentalAPI.Models.Pagination;


namespace CarRentalAPI.Controllers;

[Route("api/rentaloffices")]
[ApiController]
[Authorize(Roles = "administrator,rentalOwner")]
public class RentalsController(IRentalService rentalService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<PresentRentalOfficeDto>> GetAll([FromQuery] RentalQuery query)
    {
        return Ok(rentalService.GetRentalAll(query));
    }

    [HttpGet("{id}")]
    public ActionResult<PresentRentalOfficeDto> Get([FromRoute] int id) 
    {
        var rentalDto = rentalService.GetRentalById(id);

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

        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value);

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
