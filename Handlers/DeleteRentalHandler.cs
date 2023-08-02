using AutoMapper;
using CarRentalAPI.Entities;

namespace CarRentalAPI.Handlers
{
    public class DeleteRentalHandler : IDeleteRentalHandler
    {
        public readonly RentalDbContext dbContext;
        public readonly IMapper mapper;
        public DeleteRentalHandler(RentalDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public bool HandleDeleteRental(int id)
        {
            var rental = this.dbContext.rentalOffices
                .FirstOrDefault(r => r.Id == id);
            if (rental == null)
            {
                return false;

            }

            this.dbContext.rentalOffices.Remove(rental);
            this.dbContext.SaveChanges();

            return true;
        }
    }
}
