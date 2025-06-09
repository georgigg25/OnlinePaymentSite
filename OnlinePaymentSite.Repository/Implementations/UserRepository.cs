using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Models;
using OnlinePaymentSite.Repository.Base;
using OnlinePaymentSite.Repository.Helpers;
using OnlinePaymentSite.Repository.Interfaces.User;

namespace OnlinePaymentSite.Repository.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private const string IdDbFieldEnumeratorName = "UserId";

        public override string GetTableName() => "Users";
        public override string[] GetColumns() => new[]
        {
            IdDbFieldEnumeratorName,
            "Username",
            "Password",
            "FullName"
        };

        public override User MapEntity(SqlDataReader reader)
        {
            return new User
            {
                UserId = Convert.ToInt32(reader[IdDbFieldEnumeratorName]),
                Username = Convert.ToString(reader["Username"]),
                Password = Convert.ToString(reader["Password"]),
                FullName = Convert.ToString(reader["FullName"])
            };
        }

        public Task<int> CreateAsync(User entity)
        {
            throw new NotImplementedException();
        }

        public Task<User> RetrieveAsync(int objectId)
        {
            return base.RetrieveAsync(IdDbFieldEnumeratorName, objectId);
        }

        public IAsyncEnumerable<User> RetrieveCollectionAsync(UserFilter filter)
        {
            Filter commandFilter = new Filter();
            if (filter.Username is not null)
            {
                commandFilter.AddCondition("Username", filter.Username);
            }
            return base.RetrieveCollectionAsync(commandFilter);
        }

        public async Task<bool> UpdateAsync(int objectId, UserUpdate update)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            UpdateCommand updateCommand = new UpdateCommand(connection, "Users", IdDbFieldEnumeratorName, objectId);
            updateCommand.AddSetClause("FullName", update.FullName);
            updateCommand.AddSetClause("Password", update.Password);
            return await updateCommand.ExecuteNonQueryAsync() > 0;
        }

        public Task<bool> DeleteAsync(int objectId)
        {
            throw new NotImplementedException();
        }
    }
}
