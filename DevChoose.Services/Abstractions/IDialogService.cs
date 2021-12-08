using System.Collections.Generic;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Services.Requests;

namespace DevChoose.Services.Abstractions
{
    public interface IDialogService
    {
        public Task<IEnumerable<User>> GetCompanionsAsync(int offset, int count, string fullName);

        public Task<GetDialogRequest> GetAsync(int id, string fullName);

        public Task<Dialog> CreateAsync(Dialog dialog);

        public Task UpdateAsync(Message message);

        public Task DeleteAsync(int id);

        public Task<GetDialogRequest> WriteAsync(int receiverId, string senderFullName);
    }
}
