using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DevChoose.Domain.Models;
using DevChoose.Domain.Repositories;
using DevChoose.Services.Abstractions;
using DevChoose.Services.Exceptions;
using DevChoose.Services.Requests;
using DevChoose.Services.Responses;

namespace DevChoose.Services.Implementations
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepository<User> repository;

        public AuthorizationService(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            LoginResponse response = new();

            User userByDataSearchResult = await this.repository.FindAsync(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);

            if (userByDataSearchResult != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userByDataSearchResult.FullName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, userByDataSearchResult.Role),
                };

                ClaimsIdentity identity = new (claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                response.ClaimsIdentity = identity;

                return response;
            } else
            {
                throw new InvalidAuthDataException();
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            RegisterResponse response = new();

            User userByEmailSearchResult = await this.repository.FindAsync(u => u.Email == registerRequest.Email);

            if (userByEmailSearchResult != null)
            {
                throw new EmailAlreadyTakenException();
            }

            User newUser = new()
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Role = registerRequest.Role,
                FullName = registerRequest.FullName,
                Phone = registerRequest.Phone,
                CompanyName = registerRequest.CompanyName,
                About = registerRequest.About,
                Location = registerRequest.Location
            };

            User createdUser = await this.repository.CreateAsync(newUser);

            response.User = createdUser;

            return response;
        }
    }
}
