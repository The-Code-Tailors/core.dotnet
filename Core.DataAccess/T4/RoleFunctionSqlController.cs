using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class RoleFunctionSqlController : DataAccessSqlController<RoleFunction>
    {
        public RoleFunctionSqlController(ControllerConfiguration configuration, RoleFunction entity)
            : base(configuration, entity, "RoleFunctionDelete", "RoleFunctionInsert", "RoleFunctionSelect", "RoleFunctionSelectList", "RoleFunctionSelectVersionHistory", "RoleFunctionSelectVersionHistoryItem", "RoleFunctionUpdate")
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
            command.Parameters.AddWithValue("@Function", GetValueOrDBNull(entity.Function));
            command.Parameters.AddWithValue("@RoleId", GetValueOrDBNull(entity.RoleId));
        }

        public RoleFunction Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "RoleFunctionSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

