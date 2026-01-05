using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityColumnSqlController : DataAccessSqlController<FlexibleEntityColumn>
    {
        public FlexibleEntityColumnSqlController(ControllerConfiguration configuration, FlexibleEntityColumn entity)
            : base(configuration, entity, "FlexibleEntityColumnDelete", "FlexibleEntityColumnInsert", "FlexibleEntityColumnSelect", "FlexibleEntityColumnSelectList", "FlexibleEntityColumnSelectVersionHistory", "FlexibleEntityColumnSelectVersionHistoryItem", "FlexibleEntityColumnUpdate")
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
            command.Parameters.AddWithValue("@FlexibleColumnId", GetValueOrDBNull(entity.FlexibleColumnId));
            command.Parameters.AddWithValue("@FlexibleEntityId", GetValueOrDBNull(entity.FlexibleEntityId));
        }

        public FlexibleEntityColumn Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "FlexibleEntityColumnSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

