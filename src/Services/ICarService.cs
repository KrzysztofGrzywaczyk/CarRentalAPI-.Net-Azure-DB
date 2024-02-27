using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;
using CarRentalAPI.Models.Queries;

namespace CarRentalAPI.Services;

public interface ICarService
{
    public string CreateCar(int rentalID, CreateUpdateCarDto dto);

    public void DeleteCar(int rentalID, int carId);

    public PagedResult<PresentCarAllCarsDto> GetAllCarsInBase(CarQuery query);

    public PagedResult<PresentCarDto> GetAllCarsInRental(int rentalId, CarQuery query);

    public PresentCarDto GetCarById(int rentalId, int carId);

    public string PutCar(int rentalId, int carId, CreateUpdateCarDto dto);
}
