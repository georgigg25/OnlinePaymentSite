using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePaymentSite.Repository.Interfaces.User
{
    public class UserFilter
    {
        public SqlString? Username { get; set; }
    }
}
