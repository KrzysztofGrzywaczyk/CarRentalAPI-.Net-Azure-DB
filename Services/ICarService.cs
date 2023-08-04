using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Services
{
    public interface ICarService
    {
        public string CreateCar(int rentalID, CreateCarDto dto);
        public IEnumerable<CarDto> GetCarAll(int rentalId);
        public CarDto GetCarById(int rentalId, int carId);

    }
}
