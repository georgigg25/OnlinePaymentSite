using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Models;
using OnlinePaymentSite.Repository.Base;
using OnlinePaymentSite.Repository.Helpers;
using OnlinePaymentSite.Repository.Interfaces.Payment;

namespace OnlinePaymentSite.Repository.Implementations
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        private const string IdDbFieldEnumeratorName = "PaymentId";

        public override string GetTableName() => "Payments";
        public override string[] GetColumns() => new[]
        {
            IdDbFieldEnumeratorName,
            "FromAccountId",
            "ToAccountId",
            "Amount",
            "Reason",
            "PaymentDate"
        };

        public override Payment MapEntity(SqlDataReader reader)
        {
            return new Payment
            {
                PaymentId = Convert.ToInt32(reader[IdDbFieldEnumeratorName]),
                FromAccountId = Convert.ToInt32(reader["FromAccountId"]),
                ToAccountId = Convert.ToInt32(reader["ToAccountId"]),
                Amount = Convert.ToDecimal(reader["Amount"]),
                Reason = Convert.ToString(reader["Reason"]),
                PaymentDate = Convert.ToDateTime(reader["PaymentDate"])
            };
        }

        public Task<int> CreateAsync(Payment entity)
        {
            return base.CreateAsync(entity, IdDbFieldEnumeratorName);
        }

        public Task<Payment> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Payment> RetrieveCollectionAsync(PaymentFilter filter)
        {
            Filter commandFilter = new Filter();
            if (filter.FromAccountId is not null)
            {
                commandFilter.AddCondition("FromAccountId", filter.FromAccountId);
            }
            if (filter.ToAccountId is not null)
            {
                commandFilter.AddCondition("ToAccountId", filter.ToAccountId);
            }
            return base.RetrieveCollectionAsync(commandFilter);
        }

        public Task<bool> UpdateAsync(int objectId, PaymentUpdate update)
        {
            throw new NotImplementedException("No updatable fields for Payment");
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }
    }
}
