using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Services.DTOs.Payment
{
    public class CreatePaymentRequest
    {
        [Required(ErrorMessage = "From account is required")]
        public int FromAccountId { get; set; }

        [Required(ErrorMessage = "To account is required")]
        public int ToAccountId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [StringLength(32, ErrorMessage = "Reason cannot exceed 32 characters")]
        public string Reason { get; set; }
    }
}
