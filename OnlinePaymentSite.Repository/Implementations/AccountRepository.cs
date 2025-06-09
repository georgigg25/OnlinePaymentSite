using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Models;
using OnlinePaymentSite.Repository.Base;
using OnlinePaymentSite.Repository.Helpers;
using OnlinePaymentSite.Repository.Interfaces.Account;

namespace OnlinePaymentSite.Repository.Implementations
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        private const string IdDbFieldEnumeratorName = "AccountId";

        public override string GetTableName() => "Accounts";
        public override string[] GetColumns() => new[]
        {
            IdDbFieldEnumeratorName,
            "AccountNumber",
            "Balance"
        };

        public override Account MapEntity(SqlDataReader reader)
        {
            return new Account
            {
                AccountId = Convert.ToInt32(reader[IdDbFieldEnumeratorName]),
                AccountNumber = Convert.ToString(reader["AccountNumber"]),
                Balance = Convert.ToDecimal(reader["Balance"])
            };
        }

        public Task<int> CreateAsync(Account entity)
        {
            throw new NotImplementedException();
        }

        public Task<Account> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<Account> RetrieveCollectionAsync(AccountFilter filter)
        {
            Filter commandFilter = new Filter();
            if (filter.AccountNumber is not null)
            {
                commandFilter.AddCondition("AccountNumber", filter.AccountNumber);
            }
            return base.RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, AccountUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            UpdateCommand updateCommand = new UpdateCommand(connection, "Accounts", IdDbFieldEnumeratorName, objectId);
            updateCommand.AddSetClause("Balance", update.Balance);
            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }
    }
}
