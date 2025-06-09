using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Repository.Base;

namespace OnlinePaymentSite.Repository.Interfaces.UserAccount
{
    public interface IUserAccountRepository : IBaseRepository<Models.UserAccount, UserAccountFilter, UserAccountUpdate>
    {
        IAsyncEnumerable<Models.Account> GetAccountsForUserAsync(int userId);
    }
}
