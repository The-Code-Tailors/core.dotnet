using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class RoleSqlController : DataAccessSqlController<Role>
    {
        public RoleSqlController(ControllerConfiguration configuration, Role entity)
            : base(configuration, entity, "RoleDelete", "RoleInsert", "RoleSelect", "RoleSelectList", "RoleSelectVersionHistory", "RoleSelectVersionHistoryItem", "RoleUpdate")
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
        }

        public Role Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "RoleSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

