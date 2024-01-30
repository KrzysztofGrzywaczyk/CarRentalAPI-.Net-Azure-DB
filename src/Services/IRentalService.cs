using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;

namespace CarRentalAPI.Services;

public interface IRentalService
{
    public string CreateRental(CreateRentalOfficeDto dto);

    public void DeleteRental(int id);
    
    public PagedResult<PresentRentalOfficeDto> GetRentalAll(RentalQuery query);
    
    public PresentRentalOfficeDto GetRentalById(int id);
    
    public void PutRentalById(UpdateRentalOfficeDto dto, int id);
}
