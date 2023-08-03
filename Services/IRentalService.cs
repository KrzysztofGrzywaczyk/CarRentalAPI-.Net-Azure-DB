using CarRentalAPI.Models;

namespace CarRentalAPI.Services
{
    public interface IRentalService
    {
        public string CreateRental(CreateRentalOfficeDto dto);

        public bool DeleteRental(int id);
        
        public IEnumerable<RentalOfficeDto> GetRentalAll();
        
        public RentalOfficeDto GetRentalById(int id);
        
        public bool PutRentalById(RentalOfficeUpdateDto dto, int id);
    }
}
