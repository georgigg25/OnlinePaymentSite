using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Repository.Base;

namespace OnlinePaymentSite.Repository.Interfaces.Account
{
    public interface IAccountRepository : IBaseRepository<Models.Account, AccountFilter, AccountUpdate>
    {
    }
}
