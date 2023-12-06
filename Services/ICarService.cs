using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Services
{
    public interface ICarService
    {
        public string CreateCar(int rentalID, CreateCarDto dto);

        public void DeleteCar(int rentalID, int carId);

        public IEnumerable<PresentCarAllCarsDto> GetAllCarsInBase();

        public IEnumerable<PresentCarDto> GetAllCarsInRental(int rentalId);

        public PresentCarDto GetCarById(int rentalId, int carId);

        public string PutCar(int rentalId, int carId, CreateCarDto dto);
    }
}
