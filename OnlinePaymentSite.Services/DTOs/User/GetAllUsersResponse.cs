using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Services.DTOs.User
{
    public class GetAllUsersResponse
    {
        public List<UserInfo> Users { get; set; }
        public int TotalCount { get; set; }
    }
}
