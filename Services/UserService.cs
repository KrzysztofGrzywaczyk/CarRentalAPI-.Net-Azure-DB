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

namespace CarRentalAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly RentalDbContext _dbContext;
        private readonly IMapper _mapper;

        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(RentalDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings, IMapper mapper)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _mapper = mapper;
        }
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

            var hashedPassword = _passwordHasher.HashPassword(newUser, userDto.Password!);
            newUser.HashedPassword = hashedPassword;

            _dbContext.users.Add(newUser);
            _dbContext.SaveChanges();

            return $"/api/users/{newUser.Id}";
        }

        public void DeleteUser(int userId)
        {
            var user = _dbContext.users.FirstOrDefault(u => u.Id == userId);

            if (user is null)
            {
                throw new BadHttpRequestException("Given user id does not exist");
            }

            _dbContext.users.Remove(user);
            _dbContext.SaveChanges();
        }

        public string GenerateToken(LoginDto loginDto)
        {
            var user = _dbContext.users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == loginDto.Email);

            if (user is null)
            {
                throw new BadHttpRequestException("Invalid username or password");
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword!, loginDto.Password!);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new BadHttpRequestException("Invalid username or password");
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role!.Name),
                new Claim("DateOfBirth", user.DateofBirth.ToString("yyyy-MM-dd"))
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public List<PresentUserDto> GetAllUsers()
        {
            var users = _dbContext.users.Include(u => u.Role).ToList();
            var userDtos = _mapper.Map<List<PresentUserDto>>(users);

            return userDtos;
        }

        public PresentUserDto GetUserById(int userId)
        {
            var user = _dbContext.users.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);
            var userDto = _mapper.Map<PresentUserDto>(user);

            return userDto;

        }

        public void PutUser(UpdateUserDto userDto, int userId)
        {
            var user = _dbContext.users.Include(u => u.Role).FirstOrDefault(u => u.Id == userId);

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
                var hashedPassword = _passwordHasher.HashPassword(user, userDto.Password!);
                user.HashedPassword = hashedPassword;
            }

            _dbContext.SaveChanges();
        }
    }
}
