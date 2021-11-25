using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DevChoose.Web.Models;
using DevChoose.Services.Abstractions;
using DevChoose.Domain.Models;

namespace DevChoose.Web.Controllers
{
    [Route("developers")]
    public class DevelopersController : Controller
    {
        private readonly IUserService service;

        public DevelopersController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Developers(int skip, int take)
        {
            IEnumerable<User> developers = await this.service.GetDevelopersAsync(skip, take);
            return View("Developers", developers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Developer(int id)
        {
            User developer = await this.service.GetDeveloperAsync(id);
            return View(developer);
        }
    }
}
