using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DevChoose.Web.Controllers
{
    [Route("dialogs")]
    public class DialogsController : Controller
    {
        private readonly IDialogService dialogService;
        private readonly IMessageService messageService;

        public DialogsController(IDialogService dialogService, IMessageService messageService)
        {
            this.dialogService = dialogService;
            this.messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> Dialogs(int skip, int take)
        {
            string fullName = this.GetCurrentUserFullName();

            IEnumerable<Dialog> dialogs = await this.dialogService.GetAsync(skip, take, fullName);

            return View(dialogs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Dialog(int id, int skip, int take)
        {
            string fullName = this.GetCurrentUserFullName();

            try
            {
                Dialog dialog = await this.dialogService.GetAsync(id, fullName);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

            IEnumerable<Message> messages = await this.messageService.GetAsync(skip, take, id);

            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            IEnumerable<Message> newMessageCollection = await this.messageService.SendAsync(message);

            return View(newMessageCollection);
        }

        private string GetCurrentUserFullName()
        {
            return HttpContext.User.Identity.Name;
        }
    }
}
