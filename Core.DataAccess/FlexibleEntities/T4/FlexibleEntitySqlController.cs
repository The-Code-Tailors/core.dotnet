using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntitySqlController : DataAccessSqlController<FlexibleEntity>
    {
        public FlexibleEntitySqlController(ControllerConfiguration configuration, FlexibleEntity entity)
            : base(configuration, entity, "FlexibleEntityDelete", "FlexibleEntityInsert", "FlexibleEntitySelect", "FlexibleEntitySelectList", "FlexibleEntitySelectVersionHistory", "FlexibleEntitySelectVersionHistoryItem", "FlexibleEntityUpdate")
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

        public FlexibleEntity Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "FlexibleEntitySelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

