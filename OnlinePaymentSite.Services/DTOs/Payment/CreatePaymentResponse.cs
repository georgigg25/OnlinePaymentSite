using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Services.DTOs.Payment
{
    public class CreatePaymentResponse : PaymentInfo
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
