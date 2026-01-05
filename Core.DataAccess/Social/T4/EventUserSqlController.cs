using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class EventUserSqlController : DataAccessSqlController<EventUser>
    {
        public EventUserSqlController(ControllerConfiguration configuration, EventUser entity)
            : base(configuration, entity, "EventUserDelete", "EventUserInsert", "EventUserSelect", "EventUserSelectList", "EventUserSelectVersionHistory", "EventUserSelectVersionHistoryItem", "EventUserUpdate")
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
            command.Parameters.AddWithValue("@EventId", GetValueOrDBNull(entity.EventId));
            command.Parameters.AddWithValue("@UserId", GetValueOrDBNull(entity.UserId));
        }

        public EventUser Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "EventUserSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

