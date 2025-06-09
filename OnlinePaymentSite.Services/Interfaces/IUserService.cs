using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Services.DTOs.User;

namespace OnlinePaymentSite.Services.Interfaces
{
    public interface IUserService
    {
        Task<GetUserResponse> GetByIdAsync(int userId);
        Task<GetAllUsersResponse> GetAllAsync();
        Task<UpdateUserResponse> UpdateFullNameAsync(UpdateFullNameRequest request);
        Task<UpdateUserResponse> UpdatePasswordAsync(UpdatePasswordRequest request);
    }
}
