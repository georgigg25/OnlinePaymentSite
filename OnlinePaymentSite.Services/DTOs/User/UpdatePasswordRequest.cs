using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Services.DTOs.User
{
    public class UpdatePasswordRequest
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, ErrorMessage = "Password cannot exceed 256 characters")]
        public string NewPassword { get; set; }
    }
}
