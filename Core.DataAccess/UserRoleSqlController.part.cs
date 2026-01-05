using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserRoleSqlController
    {
        public UserRole Select(long domainId, long userId, long roleId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [UserRole] WITH (READUNCOMMITTED) WHERE [DomainId] = @DomainId AND [UserId] = @UserId AND [RoleId] = @RoleId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@RoleId", roleId);
            return Select(command);
        }

        public List<UserRole> SelectListByUserId(long domainId, long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [UserRole] WITH (READUNCOMMITTED) WHERE [DomainId] = @DomainId AND [UserId] = @UserId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            command.Parameters.AddWithValue("@UserId", userId);
            return SelectList(command);
        }

        public List<UserRole> SelectListByRoleId(long domainId, long roleId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [UserRole] WITH (READUNCOMMITTED) WHERE [DomainId] = @DomainId AND [RoleId] = @RoleId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DomainId", domainId);
            command.Parameters.AddWithValue("@RoleId", roleId);
            return SelectList(command);
        }

    }
}

