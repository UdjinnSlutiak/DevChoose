using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services.Abstractions;
using DevChoose.Services.Exceptions;
using DevChoose.Services.Requests;

namespace DevChoose.Services.Implementations
{
    public class DialogService : IDialogService
    {
        private readonly IRepository<Dialog> dialogRepository;
        private readonly IRepository<Message> messageRepository;
        private readonly IRepository<User> userRepository;

        public DialogService(IRepository<Message> messageRepository, IRepository<Dialog> dialogRepository,
            IRepository<User> userRepository)
        {
            this.messageRepository = messageRepository;
            this.dialogRepository = dialogRepository;
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetCompanionsAsync(int offset, int count, string fullName)
        {
            var currentUser = await this.GetCurrentUser(fullName);

            List<Dialog> dialogs = new();
            if (currentUser.Role == "Developer")
            {
                dialogs = (await this.dialogRepository.FilterAsync(d => d.DeveloperId == currentUser.Id)).ToList();

                var necessaryDialogs = dialogs.Skip(offset).Take(count);

                List<User> companions = new();
                foreach (var item in necessaryDialogs)
                {
                    companions.Add(await this.userRepository.GetAsync(item.CustomerId));
                }

                return companions;
            } else
            {
                dialogs = (await this.dialogRepository.FilterAsync(d => d.CustomerId == currentUser.Id)).ToList();
                var necessaryDialogs = dialogs.Skip(offset).Take(count);

                List<User> companions = new();
                foreach (var item in necessaryDialogs)
                {
                    companions.Add(await this.userRepository.GetAsync(item.DeveloperId));
                }

                return companions;
            }
        }

        public async Task<GetDialogRequest> GetAsync(int id, string fullName)
        {
            var currentUser = await this.GetCurrentUser(fullName);

            GetDialogRequest response = new();

            if (currentUser.Role == "Developer")
            {
                response.Dialog = (await this.dialogRepository.FilterAsync(d => d.DeveloperId == currentUser.Id && d.CustomerId == id)).SingleOrDefault();
            } else
            {
                response.Dialog = (await this.dialogRepository.FilterAsync(d => d.CustomerId == currentUser.Id && d.DeveloperId == id)).SingleOrDefault();
            }

            if (response.Dialog.CustomerId != currentUser.Id && response.Dialog.DeveloperId != currentUser.Id)
            {
                throw new PermissionDeniedException();
            }

            if (currentUser.Role == "Developer")
            {
                response.Companion = await this.userRepository.GetAsync(response.Dialog.CustomerId);
            } else
            {
                response.Companion = await this.userRepository.GetAsync(response.Dialog.DeveloperId);
            }

            response.Messages = await this.messageRepository.FilterAsync(m => m.DialogId == response.Dialog.Id);

            return response;
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
            await this.dialogRepository.DeleteAsync(id);
        }

        public async Task<GetDialogRequest> WriteAsync(int receiverId, string senderFullName)
        {
            var currentUser = await this.GetCurrentUser(senderFullName);

            var developer = (await this.userRepository.FilterAsync(u => u.Id == receiverId)).SingleOrDefault();
            var customer = (await this.userRepository.FilterAsync(u => u.FullName == senderFullName)).SingleOrDefault();

            Dialog checkDialogExistanceResult = new();

            checkDialogExistanceResult = (await this.dialogRepository.FilterAsync(d => d.CustomerId == currentUser.Id && d.DeveloperId == receiverId)).SingleOrDefault();

            GetDialogRequest response = new();
            if (checkDialogExistanceResult != null)
            {
                response.Dialog = checkDialogExistanceResult;
                response.Companion = developer;
                response.Messages = await this.messageRepository.FilterAsync(m => m.DialogId == checkDialogExistanceResult.Id);
            }
            else
            {
                response.Dialog = await this.CreateAsync(new Dialog { DeveloperId = developer.Id, CustomerId = customer.Id });
                response.Companion = developer;
                response.Messages = new List<Message>();
            }

            return response;
        }

        private async Task<User> GetCurrentUser(string fullName)
        {
            return (await this.userRepository.FilterAsync(u => u.FullName == fullName)).SingleOrDefault();
        }
    }
}
