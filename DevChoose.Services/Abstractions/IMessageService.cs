using System.Collections.Generic;
using System.Threading.Tasks;
using DevChoose.Domain.Models;

namespace DevChoose.Services.Abstractions
{
    public interface IMessageService
    {
        public Task<IEnumerable<Message>> GetAsync(int offset, int count, int dialogId);

        public Task<IEnumerable<Message>> SendAsync(Message message);
    }
}
