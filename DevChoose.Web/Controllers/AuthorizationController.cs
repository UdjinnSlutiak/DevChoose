using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevChoose.Services.Abstractions;
using DevChoose.Services.Requests;
using DevChoose.Services.Responses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DevChoose.Web.Controllers
{
    [Route("authorization")]
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService service;

        public AuthorizationController(IAuthorizationService service)
        {
            this.service = service;
        }

        [HttpGet("main")]
        public IActionResult Main() => View();

        [HttpGet("register")]
        public IActionResult Register() => View();

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            RegisterResponse response = new();

            try
            {
                response = await this.service.RegisterAsync(registerRequest);
            }
            catch (Exception ex)
            {
                return View("Login", ex.Message);
            }

            LoginRequest loginRequest = new()
            {
                Email = response.User.Email,
                Password = response.User.Password,
                FullName = response.User.FullName,
            };

            var loginResult = await this.service.LoginAsync(loginRequest);

            await this.Authenticate(loginResult.ClaimsIdentity);

            return RedirectToAction("Developers", "Developers", new { skip = 0, take = 20 });
        }

        [HttpGet("login")]
        public IActionResult Login() => View();

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            LoginResponse response = new();

            try
            {
                response = await this.service.LoginAsync(loginRequest);
            }
            catch (Exception ex)
            {
                return View("Login", ex.Message);
            }

            await this.Authenticate(response.ClaimsIdentity);

            return RedirectToAction("Developers", "Developers", new { skip = 0, take = 20 });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        public async Task Authenticate(ClaimsIdentity identity)
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
        }
    }
}
