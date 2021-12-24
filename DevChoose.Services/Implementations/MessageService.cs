using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services.Abstractions;

namespace DevChoose.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IRepository<Message> messageRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Dialog> dialogRepository;

        public MessageService(IRepository<Message> messageRepository, IRepository<User> userRepository,
            IRepository<Dialog> dialogRepository)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.dialogRepository = dialogRepository;
        }

        public async Task<IEnumerable<Message>> GetAsync(int offset, int count, int dialogId)
        {
            return await this.messageRepository.FilterAsync(m => m.DialogId == dialogId);
        }

        public async Task<Dialog> SendAsync(string text, int dialogId, string fullName)
        {
            var currentUser = (await this.userRepository.FilterAsync(u => u.FullName == fullName)).SingleOrDefault();

            Message message = new()
            {
                Text = text,
                Sent = System.DateTime.Now,
                DialogId = dialogId,
                SenderId = currentUser.Id
            };

            await this.messageRepository.CreateAsync(message);
            return (await this.dialogRepository.FilterAsync(d => d.Id == message.DialogId)).SingleOrDefault();
        }
    }
}
