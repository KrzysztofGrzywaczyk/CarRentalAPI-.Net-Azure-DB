using AutoMapper;
using CarRentalAPI.Authorization;
using CarRentalAPI.Entities;
using CarRentalAPI.Exceptions;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static CarRentalAPI.Entities.RentalOffice;

namespace CarRentalAPI.Services;

public class RentalService : IRentalService
{
    private const string entityName = "Rental";
    public readonly RentalDbContext _dbContext;
    public readonly IAuthorizationService _authorizationService;
    public readonly ILogger<RentalService> _logger;
    public readonly ILogHandler _logHandler;
    public readonly IMapper _mapper;
    public readonly IUserContextService _userContextService;

    public RentalService(RentalDbContext dbContext, ILogger<RentalService> logger,ILogHandler logHandler , IMapper mapper, 
        IAuthorizationService authorizationService, IUserContextService userContextService)
    {
        _authorizationService = authorizationService;
        _dbContext = dbContext;
        _logger = logger;
        _logHandler = logHandler;
        _mapper = mapper;
        _userContextService = userContextService;
    }

    public string CreateRental(CreateRentalOfficeDto dto)
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.POST);

        var rentalOffice = _mapper.Map<RentalOffice>(dto);
        rentalOffice.OwnerId = _userContextService.GetUserId;

        _dbContext.rentalOffices.Add(rentalOffice);
        _dbContext.SaveChanges();

        _logHandler.LogAction(ILogHandler.ActionEnum.Created, rentalOffice.Id);

        string path = $"/api/rentaloffices/{rentalOffice.Id}";
        return path;
    }

    public void DeleteRental(int id)
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.DELETE);
        var rentalOffice = _dbContext.rentalOffices
            .FirstOrDefault(r => r.Id == id);

        NullRentalCheck(rentalOffice!, id);

        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, rentalOffice, new ResourceOperationRequirement(ResourceOperation.Delete));

        if (!authorizationResult.Result.Succeeded)
        {
            throw new ForbidException();
        }

        _dbContext.rentalOffices.Remove(rentalOffice!);
        _dbContext.SaveChanges();

        _logHandler.LogAction(ILogHandler.ActionEnum.Deleted, rentalOffice!.Id);
    }

    public IEnumerable<PresentRentalOfficeDto> GetRentalAll()
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);
        var rentals = _dbContext.rentalOffices
            .Include(r => r.Address)
            .Include(r => r.Cars)
            .ToList();

        var rentalDtos = _mapper.Map<List<PresentRentalOfficeDto>>(rentals);

        return rentalDtos;
    }

    public PresentRentalOfficeDto GetRentalById(int id)
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.GET);
        var rentalOffice = _dbContext.rentalOffices
            .Include(r => r.Address)
            .Include(r => r.Cars)
            .FirstOrDefault(r => r.Id == id);

        NullRentalCheck(rentalOffice!, id);

        var rentalDto = _mapper.Map<PresentRentalOfficeDto>(rentalOffice);
        return rentalDto;

    }

    public void PutRentalById(UpdateRentalOfficeDto dto, int id)
    {
        _logHandler.LogNewRequest(entityName, ILogHandler.RequestEnum.PUT);

        var rentalOffice = _dbContext.rentalOffices
            .Include(r => r.Address)
            .Include(r => r.Cars)
            .FirstOrDefault(r => r.Id == id);

        NullRentalCheck(rentalOffice!, id);

        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, rentalOffice, new ResourceOperationRequirement(ResourceOperation.Update));

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

        _dbContext.SaveChanges();

        _logHandler.LogAction(ILogHandler.ActionEnum.Updated, rentalOffice.Id);
    }

    private static void NullRentalCheck(RentalOffice rentalOffice, int id)
    {
        if (rentalOffice == null)
        {
            throw new FileNotFoundException($"Rental with id {id} not found");
        }
    }
}