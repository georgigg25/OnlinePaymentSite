using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Services.DTOs.Account;

namespace OnlinePaymentSite.Services.Interfaces
{
    public interface IAccountService
    {
        Task<GetAccountResponse> GetByIdAsync(int accountId);
        Task<GetAllAccountsResponse> GetAllAsync();
        Task<GetAllAccountsResponse> GetAccountsForUserAsync(int userId);
    }
}
