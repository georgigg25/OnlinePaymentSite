using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Services.DTOs.Payment
{
    public class GetAllPaymentsResponse
    {
        public List<PaymentInfo> Payments { get; set; }
        public int TotalCount { get; set; }
    }
}
