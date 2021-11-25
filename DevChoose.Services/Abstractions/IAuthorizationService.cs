using System;
using System.Threading.Tasks;
using DevChoose.Services.Requests;
using DevChoose.Services.Responses;

namespace DevChoose.Services.Abstractions
{
    public interface IAuthorizationService
    {
        public Task<LoginResponse> LoginAsync(LoginRequest loginRequest);

        public Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest);
    }
}
