using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Services.DTOs.User
{
    public class UpdateUserResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
