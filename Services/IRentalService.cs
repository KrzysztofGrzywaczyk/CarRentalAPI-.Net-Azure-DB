using CarRentalAPI.Models;

namespace CarRentalAPI.Services
{
    public interface IRentalService
    {
        public string CreateRental(CreateRentalOfficeDto dto);

        public void DeleteRental(int id);
        
        public IEnumerable<RentalOfficeDto> GetRentalAll();
        
        public RentalOfficeDto GetRentalById(int id);
        
        public void PutRentalById(RentalOfficeUpdateDto dto, int id);
    }
}
