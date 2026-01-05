using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class ConfigSqlController : DataAccessSqlController<Config>
    {
        public ConfigSqlController(ControllerConfiguration configuration, Config entity)
            : base(configuration, entity, "ConfigDelete", "ConfigInsert", "ConfigSelect", "ConfigSelectList", "ConfigSelectVersionHistory", "ConfigSelectVersionHistoryItem", "ConfigUpdate")
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
            command.Parameters.AddWithValue("@Key", GetValueOrDBNull(entity.Key));
        }

        public Config Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "ConfigSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

