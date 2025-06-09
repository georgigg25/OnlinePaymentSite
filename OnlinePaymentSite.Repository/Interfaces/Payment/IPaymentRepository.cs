using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Repository.Base;

namespace OnlinePaymentSite.Repository.Interfaces.Payment
{
    public interface IPaymentRepository : IBaseRepository<Models.Payment, PaymentFilter, PaymentUpdate>
    {
    }
}
