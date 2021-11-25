using System.Collections.Generic;
using System.Threading.Tasks;
using DevChoose.Domain.Models;

namespace DevChoose.Services.Abstractions
{
    public interface IDialogService
    {
        public Task<IEnumerable<Dialog>> GetAsync(int offset, int count, string fullName);

        public Task<Dialog> GetAsync(int id, string fullName);

        public Task<Dialog> CreateAsync(Dialog dialog);

        public Task UpdateAsync(Message message);

        public Task DeleteAsync(int id);
    }
}
