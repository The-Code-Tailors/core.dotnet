using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserSqlController
    {
        public User Select(string username)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [User] WHERE [UName] = @UName AND [DeleteDate] IS NULL AND VersioningId IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@UName", username);
            return Select(command);
        }

        public User Select(string username, string passwordHash)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [User] WHERE [UName] = @UName AND [PHash] = @PHash AND [DeleteDate] IS NULL AND VersioningId IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@UName", username);
            command.Parameters.AddWithValue("@PHash", passwordHash);
            return Select(command);
        }

        public List<User> SelectList(long domainId, DataAccessFunction function)
        {
            SqlCommand command = new SqlCommand() { CommandText = "select [User].* from [User] join UserRole on [User].Id = UserRole.UserId join [Role] on UserRole.RoleId = [Role].Id join RoleFunction on [Role].Id = RoleFunction.RoleId where UserRole.DomainId = @DomainId and RoleFunction.[Function] = @Function and [User].DeleteDate is null and [User].VersioningId is null", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            command.Parameters.AddWithValue("@Function", function);
            return SelectList(command);
        }

        public List<User> Search(string term)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [User] WHERE (Data.query('/User/FName').value('.', 'varchar(max)') LIKE '%' + @Term + '%' OR Data.query('/User/LName').value('.', 'varchar(max)') LIKE '%' + @Term + '%') AND [DeleteDate] IS NULL AND [VersioningId] IS NULL AND Id != 1", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@Term", term);
            return SelectList(command);
        }

        public bool IsAuthorized(long domainId, long userId, DataAccessFunction function)
        {
            SqlCommand command = new SqlCommand() { CommandText = "select count(*) from UserRole join RoleFunction on UserRole.RoleId = RoleFunction.RoleId where UserRole.DomainId = @DomainId and UserRole.UserId = @UserId and RoleFunction.[Function] = @Function and UserRole.DeleteDate is null and UserRole.VersioningId is null and RoleFunction.DeleteDate is null and RoleFunction.VersioningId is null", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Function", function);

            int x = 0;

            SqlConnection connection = GetConnection();
            try
            {
                command.Connection = connection;
                connection.Open();
                x = (int)command.ExecuteScalar();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return x != 0;
        }

        public List<DataAccessFunction> SelectFunctionList(long domainId, long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "select distinct RoleFunction.[Function] from UserRole with (readuncommitted) join RoleFunction on UserRole.RoleId = RoleFunction.RoleId where UserRole.DomainId = @DomainId and UserRole.UserId = @UserId and UserRole.DeleteDate is null and UserRole.VersioningId is null and RoleFunction.DeleteDate is null and RoleFunction.VersioningId is null order by 1", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            command.Parameters.AddWithValue("@UserId", userId);

            List<DataAccessFunction> functionList = new List<DataAccessFunction>();

            SqlConnection connection = GetConnection();
            try
            {
                command.Connection = connection;
                connection.Open();
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    functionList.Add((DataAccessFunction)dataReader.GetInt64(dataReader.GetOrdinal("Function")));
                }
                dataReader.Close();
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }

            return functionList;
        }

    }
}

