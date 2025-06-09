using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Services.DTOs.Payment
{
    public class PaymentInfo
    {
        public int PaymentId { get; set; }
        public int FromAccountId { get; set; }
        public string FromAccountNumber { get; set; }
        public int ToAccountId { get; set; }
        public string ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
