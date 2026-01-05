using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class RoleFunctionSqlController
    {
        public List<RoleFunction> SelectList(long roleId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [RoleFunction] WHERE [RoleId] = @RoleId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@RoleId", roleId);
            return SelectList(command);
        }

        public RoleFunction Select(long roleId, DataAccessFunction function)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [RoleFunction] WHERE [RoleId] = @RoleId AND [Function] = @Function AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@RoleId", roleId);
            command.Parameters.AddWithValue("@Function", function);
            return Select(command);
        }

    }
}

