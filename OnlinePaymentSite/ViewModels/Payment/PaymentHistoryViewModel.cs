using OnlinePaymentSite.Services.DTOs.Payment;

namespace OnlinePaymentSite.Web.ViewModels.Payment
{
    public class PaymentHistoryViewModel
    {
        public int SelectedAccountId { get; set; }
        public List<PaymentInfo> Payments { get; set; }
    }
}
