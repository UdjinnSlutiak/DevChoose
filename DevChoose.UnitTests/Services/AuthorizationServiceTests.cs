using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services.Exceptions;
using DevChoose.Services.Implementations;
using DevChoose.Services.Requests;
using Moq;
using Xunit;

namespace DevChoose.UnitTests.Services
{
    public class AuthorizationServiceTests
    {
        [Fact]
        public async Task Can_Login_With_Valid_Data()
        {
            var testUser = this.GetTestUser();
            var loginRequest = this.GetLoginRequest(isValid: true);
            var mock = new Mock<IRepository<User>>();
            Expression<Func<User, bool>> expression = u
                => u.Email == loginRequest.Email && u.Password == loginRequest.Password;
            mock.Setup<Task<User>>(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(testUser);
            var service = new AuthorizationService(mock.Object);

            var result = await service.LoginAsync(loginRequest);

            Assert.NotNull(result);
            Assert.NotNull(result.ClaimsIdentity);
            Assert.Equal(testUser.FullName, result.ClaimsIdentity.Claims.ElementAt(0).Value);
            Assert.Equal(testUser.Role, result.ClaimsIdentity.Claims.ElementAt(1).Value);
        }

        [Fact]
        public async Task Cant_Login_With_Invalid_Data()
        {
            var testUser = this.GetTestUser();
            var loginRequest = this.GetLoginRequest(isValid: false);
            var mock = new Mock<IRepository<User>>();
            mock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(testUser);
            var service = new AuthorizationService(mock.Object);

            var result = await service.LoginAsync(loginRequest);

            Assert.NotNull(result);
            Assert.NotNull(result.ClaimsIdentity);
            Assert.Equal(testUser.FullName, result.ClaimsIdentity.Claims.ElementAt(0).Value);
            Assert.Equal(testUser.Role, result.ClaimsIdentity.Claims.ElementAt(1).Value);
        }

        [Fact]
        public async Task Can_Register_With_Valid_Data()
        {
            var testUser = this.GetTestUser();
            var registerRequest = this.GetRegisterRequest(isValid: true);
            var mock = new Mock<IRepository<User>>();
            mock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(() => null);
            mock.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(testUser);
            var service = new AuthorizationService(mock.Object);

            var result = await service.RegisterAsync(registerRequest);

            Assert.NotNull(result);
            Assert.Equal(testUser, result.User);
        }

        [Fact]
        public async Task Cant_Register_With_Invalid_Data()
        {
            var testUser = this.GetTestUser();
            var registerRequest = this.GetRegisterRequest(isValid: false);
            var mock = new Mock<IRepository<User>>();
            mock.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(testUser);
            mock.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(testUser);
            var service = new AuthorizationService(mock.Object);

            var result = await Assert.ThrowsAsync<EmailAlreadyTakenException>(() => service.RegisterAsync(registerRequest));
        }

        private User GetTestUser()
        {
            return new User()
            {
                Id = 1,
                FullName = "Eugene Slutiak",
                Password = "123456",
                Role = "Developer",
                Email = "slutiak.jenia@gmail.com",
                CompanyName = "BlazingByte",
                About = "IT company",
                Location = "Khmelnytskyi",
                Phone = "0932934483"
            };
        }

        private LoginRequest GetLoginRequest(bool isValid)
        {
            if (isValid)
            {
                return new LoginRequest()
                {
                    FullName = "Eugene Slutiak",
                    Email = "slutiak.jenia@gmail.com",
                    Password = "123456"
                };
            }
            else
            {
                return new LoginRequest()
                {
                    FullName = "Pavlo Romanov",
                    Email = "kniaz.romanov@gmail.com",
                    Password = "romanovPasha_13"
                };
            }
        }

        private RegisterRequest GetRegisterRequest(bool isValid)
        {
            if (isValid)
            {
                return new RegisterRequest()
                {
                    FullName = "Petro Mostavchuk",
                    Password = "motivaTOR",
                    Role = "Customer",
                    Email = "aleksiuk.yana@mail.ru",
                    CompanyName = "Sobaka",
                    About = "IT company",
                    Location = "Khmelnytskyi",
                    Phone = "0675493206"
                };
            }
            else
            {
                return new RegisterRequest()
                {
                    FullName = "Eugene Slutiak",
                    Password = "123456",
                    Role = "Developer",
                    Email = "slutiak.jenia@gmail.com",
                    CompanyName = "BlazingByte",
                    About = "IT company",
                    Location = "Khmelnytskyi",
                    Phone = "0932934483"
                };
            }
        }
    }
}
