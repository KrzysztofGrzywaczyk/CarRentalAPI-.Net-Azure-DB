using CarRentalAPI.Models;

namespace CarRentalAPI.Handlers
{
    public interface IPostRentalHandler
    {
        public string HandlePostRental(CreateRentalOfficeDto dto);
    }
}
