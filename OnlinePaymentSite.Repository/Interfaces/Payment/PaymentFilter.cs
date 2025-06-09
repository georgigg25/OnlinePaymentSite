using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Repository.Interfaces.Payment
{
    public class PaymentFilter
    {
        public SqlInt32? FromAccountId { get; set; }
        public SqlInt32? ToAccountId { get; set; }
    }
}
