using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserSqlController : DataAccessSqlController<User>
    {
        public UserSqlController(ControllerConfiguration configuration, User entity)
            : base(configuration, entity, "UserDelete", "UserInsert", "UserSelect", "UserSelectList", "UserSelectVersionHistory", "UserSelectVersionHistoryItem", "UserUpdate")
        {
        }

        protected override void DeleteInternal(SqlTransaction transaction, bool permanently)
        {
            if (!permanently)
            {
            }

            base.DeleteInternal(transaction, permanently);
        }

        protected override void DoParameters(SqlCommand command)
        {
            base.DoParameters(command);
            command.Parameters.AddWithValue("@PHash", GetValueOrDBNull(entity.PHash));
            command.Parameters.AddWithValue("@UName", GetValueOrDBNull(entity.UName));
        }

        public User Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "UserSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

