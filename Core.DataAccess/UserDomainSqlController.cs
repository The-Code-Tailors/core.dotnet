using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserDomainSqlController
    {
        public UserDomain Select(long domainId, long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [UserDomain] WHERE [DomainId] = @DomainId AND [UserId] = @UserId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            command.Parameters.AddWithValue("@UserId", userId);
            return Select(command);
        }

        public List<UserDomain> SelectListByDomainId(long domainId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [UserDomain] WHERE [DomainId] = @DomainId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            return SelectList(command);
        }

        public List<UserDomain> SelectListByUserId(long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [UserDomain] WHERE [UserId] = @UserId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@UserId", userId);
            return SelectList(command);
        }

    }
}

