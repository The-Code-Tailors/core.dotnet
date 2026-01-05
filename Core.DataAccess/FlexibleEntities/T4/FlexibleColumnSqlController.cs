using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleColumnSqlController : DataAccessSqlController<FlexibleColumn>
    {
        public FlexibleColumnSqlController(ControllerConfiguration configuration, FlexibleColumn entity)
            : base(configuration, entity, "FlexibleColumnDelete", "FlexibleColumnInsert", "FlexibleColumnSelect", "FlexibleColumnSelectList", "FlexibleColumnSelectVersionHistory", "FlexibleColumnSelectVersionHistoryItem", "FlexibleColumnUpdate")
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
            command.Parameters.AddWithValue("@Name", GetValueOrDBNull(entity.Name));
        }

        public FlexibleColumn Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "FlexibleColumnSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

