using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services.Abstractions;

namespace DevChoose.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> repository;

        public UserService(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<User>> GetDevelopersAsync(int offset, int count)
        {
            IEnumerable<User> developers = await this.repository.FilterAsync(u => u.Role == "Developer");
            return developers.Skip(offset).Take(count);
        }

        public async Task<User> GetDeveloperAsync(int id)
        {
            return await this.repository.GetAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            return await this.repository.CreateAsync(user);
        }

        public async Task UpdateUserAsync(User user)
        {
            await this.repository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await this.repository.DeleteAsync(id);
        }
    }
}
