using CarRentalAPI.Models;

namespace CarRentalAPI.Handlers
{
    public interface IPutRentalHandler
    {
        public bool HandlePutById(RentalOfficeUpdateDto dto, int id);
    }
}
