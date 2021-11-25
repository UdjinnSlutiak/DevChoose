using System.Collections.Generic;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services.Abstractions;
using DevChoose.Services.Exceptions;

namespace DevChoose.Services.Implementations
{
    public class DialogService : IDialogService
    {
        private readonly IRepository<Dialog> dialogRepository;
        private readonly IRepository<Message> messageRepository;
        private readonly IRepository<User> userRepository;

        public DialogService(IRepository<Message> messageRepository, IRepository<User> userRepository)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<Dialog>> GetAsync(int offset, int count, string fullName)
        {
            return await this.dialogRepository.FilterAsync(d => d.Developer.FullName == fullName || d.Customer.FullName == fullName);
        }

        public async Task<Dialog> GetAsync(int id, string fullName)
        {
            Dialog dialog = await this.dialogRepository.GetAsync(id);

            if (dialog.Customer.FullName != fullName && dialog.Developer.FullName != fullName)
            {
                throw new PermissionDeniedException();
            }

            return dialog;
        }

        public async Task<Dialog> CreateAsync(Dialog dialog)
        {
            return await this.dialogRepository.CreateAsync(dialog);
        }

        public async Task UpdateAsync(Message message)
        {
            await this.messageRepository.UpdateAsync(message);
        }

        public async Task DeleteAsync(int id)
        {
            await this.messageRepository.DeleteAsync(id);
        }
    }
}
