using CarRentalAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPI.UnitTests;

public static class RentalOfficeDbFactory
{
    
    public static RentalDbContext CreateInMemoryDatabase()
    {  
            var options = new DbContextOptionsBuilder<RentalDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .Options;

        return new RentalDbContext(options);
    }
}
