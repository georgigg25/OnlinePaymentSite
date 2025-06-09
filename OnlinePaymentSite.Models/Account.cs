using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Models
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Account number is required")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "Account number must be between 10 and 20 characters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Account number can only contain letters and numbers")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Balance is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }
    }
}
