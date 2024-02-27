using CarRentalAPI.Entities;
using CarRentalAPI.Models;
using CarRentalAPI.Models.Pagination;
using CarRentalAPI.UnitTests.Services.Contexts;
using Microsoft.AspNetCore.Http;

namespace CarRentalAPI.UnitTests.Services
{
    public class UserServiceTests : UnitTestsBase<UserServiceTestsContext>
    {
        [Fact]
        public void WhenAnyMethodCalledWithIncorrectUserId_ShouldThrowBadHttpRequestException()
        {
            // arrange 
            var incorrectUserId = 999;
            var updateUserDto = new UpdateUserDto();

            // act
            var deleteAction = () => Context.Service.DeleteUser(incorrectUserId);
            var getAction = () => Context.Service.GetUserById(incorrectUserId);
            var putAction = () => Context.Service.PutUser(updateUserDto, incorrectUserId);

            // assert
            deleteAction.Should().Throw<BadHttpRequestException>()
                .WithMessage(Context.Service.userNotExistsMessage);
            putAction.Should().Throw<BadHttpRequestException>()
                .WithMessage(Context.Service.userNotExistsMessage);
            getAction.Should().Throw<BadHttpRequestException>()
                .WithMessage(Context.Service.userNotExistsMessage);
        }

        [Fact]
        public void WhenGenerateTokenCalledWithFailedPasswordVerification_ShouldThrowBadHttpRequestExceptionWithInvalidLoginMessage()
        {
            // arrange
            var service = Context.CreateServiceWithFailedPasswordVerification();
            var incorrectPassword= "incorrectPassword";
            var loginDto = new LoginDto()
            {
                Email = Context.TestUser.Email,
                Password = incorrectPassword
            };

            // act
            var getTokenAction = () => service.GenerateToken(loginDto);

            // assert
            getTokenAction.Should().Throw<BadHttpRequestException>()
                .WithMessage(Context.Service.invalidLoginMessage);
        }

        [Fact]
        public void WhenGenerateTokenCalledWithOfNonExistentUserLogin_ShouldThrowBadHttpRequestExceptionWithInvalidLoginMessage()
        {
            // arrange
            Context.WithUserExistingInDatabase();
            var incorrectLogin = "incorrect@email.address";
            var loginDtoWithIncorrectLogin = new LoginDto()
            {
                Email = incorrectLogin,
                Password = Context.TestPassword
            };

            // act
            var getTokenAction = () => Context.Service.GenerateToken(loginDtoWithIncorrectLogin);

            // assert
            getTokenAction.Should().Throw<BadHttpRequestException>()
                .WithMessage(Context.Service.invalidLoginMessage);

        }

        [Fact]
        public void WhenAddUserCalledCorrectly_ShouldAddUserEntity()
        {
            // arrange
            var CreateUserDto = new CreateUserDto()
            {
                Email = Context.TestUser.Email,
                Password = Context.TestPassword,
                PasswordConfirmation = Context.TestPassword,
                Nickname = Context.TestUser.Nickname,
                FirstName = Context.TestUser.FirstName,
                LastName = Context.TestUser.LastName,
                DateOfBirth = Context.TestUser.DateOfBirth,
                RoleId = Context.TestUser.RoleId
            };

            // act
            Context.Service.AddUser(CreateUserDto);

            // assert
            Context.DbContext.users.First().Should().NotBeNull();
            var entity = Context.DbContext.users.First();

            entity.Should().BeOfType<User>();
            entity.Email.Should().Be(Context.TestUser.Email);
            entity.Nickname.Should().Be(Context.TestUser.Nickname);
            entity.FirstName.Should().Be(Context.TestUser.FirstName);
            entity.LastName.Should().Be(Context.TestUser.LastName);
            entity.DateOfBirth.Should().Be(Context.TestUser.DateOfBirth);
            entity.RoleId.Should().Be(Context.TestUser.RoleId);
            entity.HashedPassword.Should().Be(Context.TestUser.HashedPassword);
        }

        [Fact]
        public void WhenDeleteUserIsCalledCorrectly_ShouldDeleteExistingUser()
        {
            // arrange
            Context.WithUserExistingInDatabase();

            // act
            Context.Service.DeleteUser(Context.TestUser.Id);

            // assert
            Context.DbContext.users.FirstOrDefault().Should().BeNull();
            Context.DbContext.users.Count().Should().Be(0);
        }

        [Fact]
        public void WhenDeleteUserIsCalledWithIncorrectId_ShouldThrowBadHttpRequestExceptionWithMessage()
        {
            // arrange
            var incorrectId = 999;

            // act
            var deleteAction = () => Context.Service.DeleteUser(incorrectId);

            // assert
            deleteAction.Should().Throw<BadHttpRequestException>().WithMessage("Given user id does not exist");
        }

        [Fact]
        public void WhenGenerateTokenIsCalledCorrectly_ShouldReturnToken()
        {
            // arrange                
            var loginDto = new LoginDto()
            {
                Email = Context.TestUser.Email,
                Password = Context.TestPassword
            };

            Context.WithUserExistingInDatabase();

            // act
            var result = Context.Service.GenerateToken(loginDto);

            // assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeOfType<string>();
            result.Length.Should().Be(455);
            result.Should().NotContainAny(" ");
        }

        [Fact]
        public void WhenGetAllUsersCalledCorrectly_ShouldReturnListOfUsers()
        {
            // arrange
            var query = new UserQuery();
            Context.WithUserExistingInDatabase();

            // act
            var result = Context.Service.GetAllUsers(query);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PagedResult<PresentUserDto>>();
            result.TotalPages.Should().Be(1);
            result.Items.Should().NotBeNullOrEmpty();
            result.ItemCount.Should().Be(1);
            result.Items!.Count().Should().Be(1);
        }

        [Fact]
        public void WhenGetByIdCalledCorrectly_ShouldReturnUser()
        {
            // arrange
            Context.WithUserExistingInDatabase();

            // act
            var result = Context.Service.GetUserById(Context.TestUser.Id);

            // assert
            result.Should().NotBeNull();
            result.Should().BeOfType<PresentUserDto>();
            result.Id.Should().Be(Context.TestUser.Id);
            result.Email.Should().Be(Context.TestUser.Email);
            result.Nickname.Should().Be(Context.TestUser.Nickname);
            result.RoleName.Should().Be(Context.TestUser.Role?.Name);      
        }
    }
}
