using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityInstanceSqlController : DataAccessSqlController<FlexibleEntityInstance>
    {
        public FlexibleEntityInstanceSqlController(ControllerConfiguration configuration, FlexibleEntityInstance entity)
            : base(configuration, entity, "FlexibleEntityInstanceDelete", "FlexibleEntityInstanceInsert", "FlexibleEntityInstanceSelect", "FlexibleEntityInstanceSelectList", "FlexibleEntityInstanceSelectVersionHistory", "FlexibleEntityInstanceSelectVersionHistoryItem", "FlexibleEntityInstanceUpdate")
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
            command.Parameters.AddWithValue("@FlexibleEntityId", GetValueOrDBNull(entity.FlexibleEntityId));
        }

        public FlexibleEntityInstance Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "FlexibleEntityInstanceSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

