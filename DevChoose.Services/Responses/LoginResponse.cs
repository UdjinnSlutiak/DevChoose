using System.Security.Claims;
using DevChoose.Domain.Models;

namespace DevChoose.Services.Responses
{
    public class LoginResponse
    {
        public User User { get; set; }
        public ClaimsIdentity ClaimsIdentity { get; set; }
    }
}
