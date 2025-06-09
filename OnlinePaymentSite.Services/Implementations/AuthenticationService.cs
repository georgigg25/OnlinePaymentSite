using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OnlinePaymentSite.Repository.Implementations;
using OnlinePaymentSite.Repository.Interfaces.User;
using OnlinePaymentSite.Services.Authentication;
using OnlinePaymentSite.Services.Helpers;
using OnlinePaymentSite.Services.Interfaces;

namespace OnlinePaymentSite.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Username and password are required"
                };
            }

            if (request.Username.Length < 3 || !Regex.IsMatch(request.Username, @"^[a-zA-Z0-9._-]+$"))
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid username format"
                };
            }

            var hashedPassword = SecurityHelper.HashPassword(request.Password);
            var filter = new UserFilter { Username = new SqlString(request.Username) };

            var users = await _userRepository.RetrieveCollectionAsync(filter).ToListAsync();
            var user = users.SingleOrDefault();

            if (user == null || user.Password != hashedPassword)
            {
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Invalid username or password"
                };
            }

            return new LoginResponse
            {
                Success = true,
                UserId = user.UserId,
                FullName = user.FullName
            };
        }
    }
}
