using Xunit;
using Moq;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services.Implementations;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;

namespace DevChoose.UnitTests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task Can_Create_User()
        {
            var testUser = this.GetTestUser();
            var mock = new Mock<IRepository<User>>();
            mock.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(testUser);
            var service = new UserService(mock.Object);

            var result = await service.CreateUserAsync(testUser);

            Assert.NotNull(result);
            Assert.Equal(testUser, result);
        }

        [Fact]
        public async Task Can_Get_Developers()
        {
            var offset = 0;
            var count = 5;
            var testUser = this.GetTestUser();
            var users = new List<User>();
            users.Add(testUser);
            var mock = new Mock<IRepository<User>>();
            mock.Setup(repo => repo.FilterAsync(It.IsAny<Expression<Func<User, bool>>>())).ReturnsAsync(users.AsEnumerable());
            var service = new UserService(mock.Object);

            var result = await service.GetDevelopersAsync(offset, count);

            Assert.NotNull(result);
            Assert.Contains(testUser, result);
        }

        [Fact]
        public async Task Can_Update_User()
        {
            var testUser = this.GetTestUser();
            var mock = new Mock<IRepository<User>>();
            mock.Setup(repo => repo.UpdateAsync(It.IsAny<User>()));
            var service = new UserService(mock.Object);

            await service.UpdateUserAsync(testUser);
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
    }
}
