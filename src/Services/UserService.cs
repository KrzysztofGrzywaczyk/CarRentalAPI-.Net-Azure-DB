using AutoMapper;
using CarRentalAPI.Configuration;
using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRentalAPI.Services;

public class UserService(RentalDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper) : IUserService
{
    public string AddUser(CreateUserDto userDto)
    {
        var newUser = new User()
        {
            Email = userDto.Email,
            Nickname = userDto.Nickname,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            DateofBirth = userDto.DateOfBirth,
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
            throw new BadHttpRequestException("Given user id does not exist");
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
            throw new BadHttpRequestException("Invalid username or password");
        }

        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.HashedPassword!, loginDto.Password!);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new BadHttpRequestException("Invalid username or password");
        }

        var claims = new List<Claim>() {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role!.Name!),
            new Claim("DateOfBirth", user.DateofBirth.ToString("yyyy-MM-dd"))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey!));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

        var token = new JwtSecurityToken(authenticationSettings.JwtIssuer, authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public List<PresentUserDto> GetAllUsers()
    {
        var users = dbContext.users.Include(u => u.Role).ToList();
        var userDtos = mapper.Map<List<PresentUserDto>>(users);

        return userDtos;
    }

    public PresentUserDto GetUserById(int userId)
    {
        var user = dbContext.users.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);
        var userDto = mapper.Map<PresentUserDto>(user);

        return userDto;

    }

    public void PutUser(UpdateUserDto userDto, int userId)
    {
        var user = dbContext.users.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);

        if (user is null)
        {
            throw new BadHttpRequestException("Invalid username or password");
        }

        user.Email = userDto.Email;
        user.Nickname = userDto.Nickname;
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.DateofBirth = userDto.DateOfBirth;
        user.RoleId = userDto.RoleId;

        if (userDto.Password is not null)
        {
            var hashedPassword = passwordHasher.HashPassword(user, userDto.Password!);
            user.HashedPassword = hashedPassword;
        }

        dbContext.SaveChanges();
    }
}
