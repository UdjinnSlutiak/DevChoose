using System.Collections.Generic;
using System.Threading.Tasks;
using DevChoose.Domain.Models;

namespace DevChoose.Services.Abstractions
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetDevelopersAsync(int offset, int count);

        public Task<User> GetDeveloperAsync(int id);

        public Task<User> CreateUserAsync(User user);

        public Task UpdateUserAsync(User user);

        public Task DeleteUserAsync(int id);
    }
}
