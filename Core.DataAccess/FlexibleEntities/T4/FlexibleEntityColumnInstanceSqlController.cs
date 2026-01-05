using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityColumnInstanceSqlController : DataAccessSqlController<FlexibleEntityColumnInstance>
    {
        public FlexibleEntityColumnInstanceSqlController(ControllerConfiguration configuration, FlexibleEntityColumnInstance entity)
            : base(configuration, entity, "FlexibleEntityColumnInstanceDelete", "FlexibleEntityColumnInstanceInsert", "FlexibleEntityColumnInstanceSelect", "FlexibleEntityColumnInstanceSelectList", "FlexibleEntityColumnInstanceSelectVersionHistory", "FlexibleEntityColumnInstanceSelectVersionHistoryItem", "FlexibleEntityColumnInstanceUpdate")
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
            command.Parameters.AddWithValue("@FlexibleEntityInstanceId", GetValueOrDBNull(entity.FlexibleEntityInstanceId));
            command.Parameters.AddWithValue("@XmlValue", GetValueOrDBNull(entity.XmlValue));
        }

        public FlexibleEntityColumnInstance Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "FlexibleEntityColumnInstanceSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

