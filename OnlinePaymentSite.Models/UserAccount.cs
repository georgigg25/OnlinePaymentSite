using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Models
{
    public class UserAccount
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int AccountId { get; set; }
    }
}
