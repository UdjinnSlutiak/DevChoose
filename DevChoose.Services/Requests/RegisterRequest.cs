using DevChoose.Domain.Models;

namespace DevChoose.Services.Requests
{
    public class RegisterRequest
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string About { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
    }
}
