using AutoMapper;
using CarRentalAPI.Configuration;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;
using CarRentalAPI.Models.Queries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace CarRentalAPI.Services;

public class UserService(RentalDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper) : IUserService
{
    public readonly string invalidLoginMessage = "Invalid username or password";

    public readonly string userNotExistsMessage = "Given user id does not exist";

    public string AddUser(CreateUserDto userDto)
    {
        var newUser = new User()
        {
            Email = userDto.Email,
            Nickname = userDto.Nickname,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            DateOfBirth = userDto.DateOfBirth,
            RoleId = userDto.RoleId
        };

        var hashedPassword = passwordHasher.HashPassword(newUser, userDto.Password!);
        newUser.HashedPassword = hashedPassword;

        dbContext.users.Add(newUser);
        dbContext.SaveChanges();

        return $"/api/users/{newUser.Id}";
    }

    public void DeleteUser(int userId)
    {
        var user = dbContext.users.FirstOrDefault(u => u.Id == userId);

        if (user is null)
        {
            throw new BadHttpRequestException(userNotExistsMessage);
        }

        dbContext.users.Remove(user);
        dbContext.SaveChanges();
    }

    public string GenerateToken(LoginDto loginDto)
    {
        var user = dbContext.users
            .Include(u => u.Role)
            .FirstOrDefault(u => u.Email == loginDto.Email);

        if (user is null)
        {
            throw new BadHttpRequestException(invalidLoginMessage);
        }

        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.HashedPassword!, loginDto.Password!);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new BadHttpRequestException(invalidLoginMessage);
        }

        var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role!.Name!),
            new Claim("DateOfBirth", user.DateOfBirth.ToString("yyyy-MM-dd"))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey!));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(authenticationSettings.JwtIssuer, authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public PagedResult<PresentUserDto> GetAllUsers(UserQuery query)
    {
        var fullQuery = dbContext.
            users.Where(u => query.SearchPhrase == null ||
            (u.FirstName!.ToLower().Contains(query.SearchPhrase.ToLower())) ||
            (u.Email!.ToLower().Contains(query.SearchPhrase.ToLower())) ||
            (u.Nickname!.ToLower().Contains(query.SearchPhrase.ToLower())));

        var count = fullQuery.Count();

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var selector = new Dictionary<string, Expression<Func<User, object>>>
            {
                { nameof(User.FirstName), u => u.FirstName! },
                { nameof(User.Email), u => u.Email! },
                { nameof(User.Nickname), u => u.Nickname! },
                { nameof(User.LastName), u => u.LastName! },
                { nameof(User.DateOfBirth), u => u.DateOfBirth! },
                { nameof(User.RoleId), u => u.RoleId! }
            };

            var selectedColumn = selector[query.SortBy];

            fullQuery = query.SortDirection == SortDirection.Ascending ? fullQuery.OrderBy(selectedColumn) : fullQuery.OrderByDescending(selectedColumn);
        }

        var pagedQuery = fullQuery
           .Skip(query.PageSize * (query.PageNumber - 1))
           .Take(query.PageSize)
           .ToList();

        var userDtos = mapper.Map<List<PresentUserDto>>(pagedQuery);
        
        var pagedResult = new PagedResult<PresentUserDto>(userDtos, count, query.PageSize, query.PageNumber);

        return pagedResult;
    }

    public PresentUserDto GetUserById(int userId)
    {
        var user = dbContext.users.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);

        if (user is null)
        {
            throw new BadHttpRequestException(userNotExistsMessage);
        }

        var userDto = mapper.Map<PresentUserDto>(user);

        return userDto;

    }

    public void PutUser(UpdateUserDto userDto, int userId)
    {
        var user = dbContext.users.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);

        if (user is null)
        {
            throw new BadHttpRequestException(userNotExistsMessage);
        }

        user.Email = userDto.Email;
        user.Nickname = userDto.Nickname;
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.DateOfBirth = userDto.DateOfBirth;
        user.RoleId = userDto.RoleId;

        if (userDto.Password is not null)
        {
            var hashedPassword = passwordHasher.HashPassword(user, userDto.Password!);
            user.HashedPassword = hashedPassword;
        }

        dbContext.SaveChanges();
    }
}
