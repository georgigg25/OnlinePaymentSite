using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Repository.Interfaces.User;
using OnlinePaymentSite.Services.DTOs.User;
using OnlinePaymentSite.Services.Helpers;
using OnlinePaymentSite.Services.Interfaces;

namespace OnlinePaymentSite.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<GetUserResponse> GetByIdAsync(int userId)
        {
            var user = await _userRepository.RetrieveAsync(userId);
            return new GetUserResponse
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName
            };
        }

        public async Task<GetAllUsersResponse> GetAllAsync()
        {
            var users = await _userRepository.RetrieveCollectionAsync(new UserFilter()).ToListAsync();
            return new GetAllUsersResponse
            {
                Users = users.Select(u => new UserInfo
                {
                    UserId = u.UserId,
                    FullName = u.FullName
                }).ToList(),
                TotalCount = users.Count
            };
        }

        public async Task<UpdateUserResponse> UpdateFullNameAsync(UpdateFullNameRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NewFullName) || request.NewFullName.Length < 2 || request.NewFullName.Length > 100)
                    throw new ValidationException("Full name must be between 2 and 100 characters");

                var update = new UserUpdate { FullName = new SqlString(request.NewFullName) };
                var success = await _userRepository.UpdateAsync(request.UserId, update);

                return new UpdateUserResponse
                {
                    Success = success,
                    UpdatedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                return new UpdateUserResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    UpdatedAt = DateTime.Now
                };
            }
        }

        public async Task<UpdateUserResponse> UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.NewPassword) || request.NewPassword.Length > 256)
                    throw new ValidationException("Password cannot be empty or exceed 256 characters");

                var hashedPassword = SecurityHelper.HashPassword(request.NewPassword);
                var update = new UserUpdate { Password = new SqlString(hashedPassword) };
                var success = await _userRepository.UpdateAsync(request.UserId, update);

                return new UpdateUserResponse
                {
                    Success = success,
                    UpdatedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                return new UpdateUserResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    UpdatedAt = DateTime.Now
                };
            }
        }
    }
}
