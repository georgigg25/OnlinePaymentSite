using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePaymentSite.Repository.Helpers;

namespace OnlinePaymentSite.Repository.Base
{
    public abstract class BaseRepository<TObj> where TObj : class
    {
        public abstract string GetTableName();
        public abstract string[] GetColumns();
        public virtual string SelectAllCommandText()
        {
            var columns = string.Join(", ", GetColumns());
            return $"SELECT {columns} FROM {GetTableName()}";
        }
        public abstract TObj MapEntity(SqlDataReader reader);

        public async Task<int> CreateAsync(TObj entity, string idDbFieldEnumeratorName = null)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();

            var properties = typeof(TObj).GetProperties()
                .Where(p => p.Name != idDbFieldEnumeratorName)
                .ToList();

            string columns = string.Join(", ", properties.Select(p => p.Name));
            string parameters = string.Join(", ", properties.Select(p => "@" + p.Name));

            command.CommandText = $@"INSERT INTO {GetTableName()} ({columns}) 
                                    VALUES ({parameters});
                                    SELECT CAST(SCOPE_IDENTITY() as int)";

            foreach (var prop in properties)
            {
                command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entity) ?? DBNull.Value);
            }

            return Convert.ToInt32(await command.ExecuteScalarAsync());
        }

        public async Task<TObj> RetrieveAsync(string idDbFieldName, int idDbFieldValue)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand sqlCommand = connection.CreateCommand();

            sqlCommand.CommandText = $"{SelectAllCommandText()} WHERE {idDbFieldName} = @{idDbFieldName}";
            sqlCommand.Parameters.AddWithValue($"@{idDbFieldName}", idDbFieldValue);
            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            if (reader.Read())
            {
                TObj result = MapEntity(reader);
                if (reader.Read())
                {
                    throw new Exception("Multiple records found for the same ID.");
                }
                return result;
            }
            throw new Exception("No record found for the given ID.");
        }

        public async IAsyncEnumerable<TObj> RetrieveCollectionAsync(Filter filter)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand sqlCommand = connection.CreateCommand();

            sqlCommand.CommandText = $"{SelectAllCommandText()} WHERE 1 = 1";
            foreach (var condition in filter.Conditions)
            {
                sqlCommand.CommandText += $" AND {condition.Key} = @{condition.Key}";
                sqlCommand.Parameters.AddWithValue($"@{condition.Key}", condition.Value);
            }

            using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                yield return MapEntity(reader);
            }
        }

        public async Task<bool> DeleteAsync(string idDbFieldName, int idDbFieldValue)
        {
            using SqlConnection connection = await ConnectionFactory.CreateConnectionAsync();
            using SqlCommand command = connection.CreateCommand();
            using SqlTransaction transaction = command.Connection.BeginTransaction();

            command.CommandText = $"DELETE FROM {GetTableName()} WHERE {idDbFieldName} = @{idDbFieldName}";
            command.Parameters.AddWithValue($"@{idDbFieldName}", idDbFieldValue);
            command.Transaction = transaction;

            int rowsAffected = await command.ExecuteNonQueryAsync();
            if (rowsAffected != 1)
            {
                throw new Exception($"Just one row should be deleted! Command aborted, because {rowsAffected} could have been deleted!");
            }

            transaction.Commit();
            return true;
        }
    }
}
