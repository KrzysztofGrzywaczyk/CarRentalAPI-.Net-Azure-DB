using AutoMapper;
using CarRentalAPI.Authorization;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;
using CarRentalAPI.Models.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Claims;
using static CarRentalAPI.Entities.RentalOffice;

namespace CarRentalAPI.Services;

public class RentalService(RentalDbContext dbContext, ILogHandler logHandler, IMapper mapper,
        IAuthorizationService authorizationService, IUserContextService userContextService) : IRentalService
{
    private const string entityName = "Rental";

    public string CreateRental(CreateRentalOfficeDto dto)
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.POST);

        var rentalOffice = mapper.Map<RentalOffice>(dto);
        rentalOffice.OwnerId = userContextService.GetUserId;

        dbContext.rentalOffices.Add(rentalOffice);
        dbContext.SaveChanges();

        logHandler.LogAction(ILogHandler.ActionEnum.Created, rentalOffice.Id);

        string path = $"/api/rentaloffices/{rentalOffice.Id}";
        return path;
    }

    public void DeleteRental(int id)
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.DELETE);
        var rentalOffice = dbContext.rentalOffices
            .FirstOrDefault(r => r.Id == id);

        NullRentalCheck(rentalOffice!, id);

        var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, rentalOffice, new ResourceOperationRequirement(ResourceOperation.Delete));

        if (!authorizationResult.Result.Succeeded)
        {
            throw new ForbidException();
        }

        dbContext.rentalOffices.Remove(rentalOffice!);
        dbContext.SaveChanges();

        logHandler.LogAction(ILogHandler.ActionEnum.Deleted, rentalOffice!.Id);
    }

    public PagedResult<PresentRentalOfficeDto> GetRentalAll(RentalQuery query)
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);

        var fullQuery = dbContext.
            rentalOffices
            .Include(r => r.Address)
            .Include(r => r.Cars)
            .Where(r => query.SearchPhrase == null ||
            (r.Name!.ToLower().Contains(query.SearchPhrase.ToLower()) || r.Description!.ToLower().Contains(query.SearchPhrase.ToLower())));

        var count = fullQuery.Count();

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var selector = new Dictionary<string, Expression<Func<RentalOffice, object>>>
            {
                { nameof(RentalOffice.Name), r => r.Name! },
                { nameof(RentalOffice.Category), r => r.Category! },
                { nameof(RentalOffice.AcceptUnder23), r => r.AcceptUnder23 },
                { nameof(RentalOffice.AddressID), r => r.AddressID }
            };

            var selectedColumn = selector[query.SortBy];

            fullQuery = query.SortDirection == SortDirection.Ascending ? fullQuery.OrderBy(selectedColumn) : fullQuery.OrderByDescending(selectedColumn);
        }

        var pagedQuery = fullQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();

        var rentalDtos = mapper.Map<List<PresentRentalOfficeDto>>(pagedQuery);

        var pagedResult = new PagedResult<PresentRentalOfficeDto>(rentalDtos, count, query.PageSize, query.PageNumber);

        return pagedResult;
    }

    public PresentRentalOfficeDto GetRentalById(int id)
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);
        var rentalOffice = dbContext.rentalOffices
            .Include(r => r.Address)
            .Include(r => r.Cars)
            .FirstOrDefault(r => r.Id == id);

        NullRentalCheck(rentalOffice!, id);

        var rentalDto = mapper.Map<PresentRentalOfficeDto>(rentalOffice);
        return rentalDto;

    }

    public void PutRentalById(UpdateRentalOfficeDto dto, int id)
    {
        logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.PUT);

        var rentalOffice = dbContext.rentalOffices
            .Include(r => r.Address)
            .Include(r => r.Cars)
            .FirstOrDefault(r => r.Id == id);

        NullRentalCheck(rentalOffice!, id);

        var authorizationResult = authorizationService.AuthorizeAsync(userContextService.User, rentalOffice, new ResourceOperationRequirement(ResourceOperation.Update));

        if (!authorizationResult.Result.Succeeded)
        {
            throw new ForbidException();
        }

        rentalOffice!.Name = dto.Name;
        rentalOffice.Description = dto.Description;
        rentalOffice.Category = (RentalCategory)Enum.Parse(typeof(RentalCategory), dto.Category!, true);
        rentalOffice.AcceptUnder23 = dto.AcceptUnder23;
        rentalOffice.ConntactEmail = dto.ConntactEmail;
        rentalOffice.ConntactNumber = dto.ConntactNumber;

        dbContext.SaveChanges();

        logHandler.LogAction(ILogHandler.ActionEnum.Updated, rentalOffice.Id);
    }

    private static void NullRentalCheck(RentalOffice rentalOffice, int id)
    {
        if (rentalOffice == null)
        {
            throw new FileNotFoundException($"Rental with id {id} not found");
        }
    }
}