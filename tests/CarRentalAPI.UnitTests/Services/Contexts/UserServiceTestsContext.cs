using AutoMapper;
using CarRentalAPI.Configuration;
using CarRentalAPI.Entities;
using CarRentalAPI.Handlers;
using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CarRentalAPI.UnitTests.Services.Contexts
{
    public class UserServiceTestsContext : UnitTestsContextBase
    {
        public UserServiceTestsContext()
        {
            DbContext = RentalOfficeDbFactory.CreateInMemoryDatabase();
            Service = CreateUserServiceFromMocks();
        }

        public RentalDbContext DbContext { get; set; }

        public UserService Service { get; set; }

        public User TestUser { get; set; } = new ()
        {
            Id = 1,
            Email = "test@email.address",
            Nickname = "TestNickname",
            FirstName = "TestName",
            LastName = "TestName",
            DateOfBirth = DateTime.Parse("01-01-1990"),
            HashedPassword = "hashedPassword",
            RoleId = 1,
            Role = new Role
            {
                Id = 1,
                Name = "Administrator"
            }
        };

        public readonly string TestPassword = "password";
        public readonly string TestRoleName = "Administrator";

        private UserService CreateUserServiceFromMocks()
        {
            var hasherMock = new Mock<IPasswordHasher<User>>();
            var authenticationSettings = new AuthenticationSettings()
            {
                JwtKey = "8313CD11BCC113368D77EC953872A"
            };
            var mapperMock = new Mock<IMapper>();

            // set password hash and verify
            hasherMock.Setup(m => m.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns($"{TestUser.HashedPassword}");
            hasherMock.Setup(m => m.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);

            // set Create mapping
            mapperMock.Setup(m => m.Map<User>(It.IsAny<CreateUserDto>())).Returns((CreateUserDto dto) =>
            {
                return new User
                {
                    Email = dto.Email,
                    Nickname = dto.Nickname,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    DateOfBirth = dto.DateOfBirth,
                    RoleId = dto.RoleId
                };              
            });

            // set Get mapping
            mapperMock.Setup(m => m.Map<PresentUserDto>(It.IsAny<User>())).Returns((User user) =>
            {
                return new PresentUserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nickname = user.Nickname,
                    RoleName = user.Role?.Name
                };
            });

            // set GetAll mapping
            mapperMock.Setup(m => m.Map<List<PresentUserDto>>(It.IsAny<List<User>>())).Returns((List<User> users) =>
            {
                return new List<PresentUserDto>
                {
                    new PresentUserDto
                    {
                        Id = users.First().Id,
                        Email = users.First().Email,
                        Nickname = users.First().Nickname,
                        RoleName = users.First().Role?.Name
                    }
                };
            });

            return new UserService(DbContext, hasherMock.Object, authenticationSettings, mapperMock.Object);
        }

        public UserService CreateServiceWithFailedPasswordVerification()
        {
            // arrange
            var hasherMock = new Mock<IPasswordHasher<User>>();
            var authenticationSettings = new AuthenticationSettings()
            {
                JwtKey = "8313CD11BCC113368D77EC953872A"
            };
            var mapperMock = new Mock<IMapper>();

            // set password hash and verify
            hasherMock.Setup(m => m.HashPassword(It.IsAny<User>(), It.IsAny<string>())).Returns($"{TestUser.HashedPassword}");
            hasherMock.Setup(m => m.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Failed);

            return new UserService(DbContext, hasherMock.Object, authenticationSettings, mapperMock.Object);
        }

        public void WithUserExistingInDatabase()
        {
            var user = new User()
            {
                Email = TestUser.Email,
                Nickname = TestUser.Nickname,
                FirstName = TestUser.FirstName,
                LastName = TestUser.LastName,
                DateOfBirth = TestUser.DateOfBirth,
                HashedPassword = TestUser.HashedPassword,
                RoleId = TestUser.RoleId,
                Role = new Role()
                {
                    Id = TestUser.Role.Id,
                    Name = TestRoleName
                }
            };

            DbContext.users.Add(user);
            DbContext.SaveChanges();
        }
    }
}
