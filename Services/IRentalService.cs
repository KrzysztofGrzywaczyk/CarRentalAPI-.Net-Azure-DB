using CarRentalAPI.Models;
using System.Security.Claims;

namespace CarRentalAPI.Services;

public interface IRentalService
{
    public string CreateRental(CreateRentalOfficeDto dto);

    public void DeleteRental(int id);
    
    public IEnumerable<PresentRentalOfficeDto> GetRentalAll();
    
    public PresentRentalOfficeDto GetRentalById(int id);
    
    public void PutRentalById(UpdateRentalOfficeDto dto, int id);
}
