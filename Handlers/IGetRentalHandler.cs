using CarRentalAPI.Models;

namespace CarRentalAPI.Handlers
{
    public interface IGetRentalHandler
    {
        public IEnumerable<RentalOfficeDto> HandleGetAllRequest();
        public RentalOfficeDto HandleGetByIdRequest(int id);
    }
}
