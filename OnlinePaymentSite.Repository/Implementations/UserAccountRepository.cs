using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Models;
using OnlinePaymentSite.Repository.Base;
using OnlinePaymentSite.Repository.Helpers;
using OnlinePaymentSite.Repository.Interfaces.UserAccount;

namespace OnlinePaymentSite.Repository.Implementations
{
    public class UserAccountRepository : BaseRepository<UserAccount>, IUserAccountRepository
    {
        private const string IdDbFieldEnumeratorName = "UserId,AccountId";

        public override string GetTableName() => "UserAccounts";
        public override string[] GetColumns() => new[] { "UserId", "AccountId" };

        public override UserAccount MapEntity(SqlDataReader reader)
        {
            return new UserAccount
            {
                UserId = Convert.ToInt32(reader["UserId"]),
                AccountId = Convert.ToInt32(reader["AccountId"])
            };
        }

        public Task<int> CreateAsync(UserAccount entity)
        {
            return base.CreateAsync(entity);
        }

        public async Task<UserAccount> RetrieveAsync(int objectId)
        {
            throw new NotImplementedException("Retrieve by single ID not applicable for composite key");
        }

        public IAsyncEnumerable<UserAccount> RetrieveCollectionAsync(UserAccountFilter filter)
        {
            Filter commandFilter = new Filter();
            if (filter.UserId is not null)
            {
                commandFilter.AddCondition("UserId", filter.UserId);
            }
            if (filter.AccountId is not null)
            {
                commandFilter.AddCondition("AccountId", filter.AccountId);
            }
            return base.RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, UserAccountUpdate update)
        {
            throw new NotImplementedException("No updatable fields for UserAccount");
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }

        public async IAsyncEnumerable<Account> GetAccountsForUserAsync(int userId)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT a.AccountId, a.AccountNumber, a.Balance
                FROM Accounts a
                INNER JOIN UserAccounts ua ON a.AccountId = ua.AccountId
                WHERE ua.UserId = @UserId";
            command.Parameters.AddWithValue("@UserId", userId);

            using SqlDataReader reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return new Account
                {
                    AccountId = Convert.ToInt32(reader["AccountId"]),
                    AccountNumber = Convert.ToString(reader["AccountNumber"]),
                    Balance = Convert.ToDecimal(reader["Balance"])
                };
            }
        }
    }
}
