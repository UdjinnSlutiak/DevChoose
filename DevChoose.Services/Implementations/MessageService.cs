using System.Collections.Generic;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services.Abstractions;

namespace DevChoose.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> messageRepository;
        private readonly IRepository<Dialog> dialogRepository;

        public MessageService(IRepository<Message> messageRepository, IRepository<Dialog> dialogRepository)
        {
            this.messageRepository = messageRepository;
            this.dialogRepository = dialogRepository;
        }

        public async Task<IEnumerable<Message>> GetAsync(int offset, int count, int dialogId)
        {
            return await this.messageRepository.FilterAsync(m => m.DialogId == dialogId);
        }

        public async Task<IEnumerable<Message>> SendAsync(Message message)
        {
            await this.messageRepository.CreateAsync(message);
            return await this.messageRepository.FilterAsync(m => m.DialogId == message.DialogId);
        }
    }
}
