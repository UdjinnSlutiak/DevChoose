using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Services.Abstractions;
using DevChoose.Services.Requests;
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

            IEnumerable<User> companions = await this.dialogService.GetCompanionsAsync(skip, take, fullName);

            return View(companions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Dialog(int id)
        {
            string fullName = this.GetCurrentUserFullName();

            GetDialogRequest response = new();
            try
            {
                response = await this.dialogService.GetAsync(id, fullName);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }

            return View("Dialog", response);
        }

        [HttpGet("write")]
        public async Task<IActionResult> Write(int id)
        {
            var fullName = this.GetCurrentUserFullName();

            var response = await this.dialogService.WriteAsync(id, fullName);

            return View("Dialog", response);
        }

        [HttpPost("send/{id}")]
        public async Task<IActionResult> SendMessage(string text, [FromRoute] int id)
        {
            var fullName = this.GetCurrentUserFullName();
            var userRole = this.GetCurrentUserRole();

            Dialog dialog = await this.messageService.SendAsync(text, id, fullName);

            if (userRole == "Developer")
            {
                return await this.Dialog(dialog.CustomerId);
            }
            else
            {
                return await this.Dialog(dialog.DeveloperId);
            }
        }

        private string GetCurrentUserFullName()
        {
            return HttpContext.User.Identity.Name;
        }

        private string GetCurrentUserRole()
        {
            return HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
        }
    }
}
