using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Services.DTOs.Payment;

namespace OnlinePaymentSite.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request);
        Task<GetAllPaymentsResponse> GetPaymentsForAccountAsync(int accountId);
    }
}
