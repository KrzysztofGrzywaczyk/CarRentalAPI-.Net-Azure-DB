using CarRentalAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Services
{
    public interface ICarService
    {
        public string CreateCar(int rentalID, CreateCarDto dto);
    }
}
