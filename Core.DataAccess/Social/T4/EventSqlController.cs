using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class EventSqlController : DataAccessSqlController<Event>
    {
        public EventSqlController(ControllerConfiguration configuration, Event entity)
            : base(configuration, entity, "EventDelete", "EventInsert", "EventSelect", "EventSelectList", "EventSelectVersionHistory", "EventSelectVersionHistoryItem", "EventUpdate")
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
            command.Parameters.AddWithValue("@BeginDate", GetValueOrDBNull(entity.BeginDate));
            command.Parameters.AddWithValue("@Description", GetValueOrDBNull(entity.Description));
            command.Parameters.AddWithValue("@EndDate", GetValueOrDBNull(entity.EndDate));
            command.Parameters.AddWithValue("@MasterEntity", GetValueOrDBNull(entity.MasterEntity));
            command.Parameters.AddWithValue("@MasterGuid", GetValueOrDBNull(entity.MasterGuid));
            command.Parameters.AddWithValue("@MasterId", GetValueOrDBNull(entity.MasterId));
            command.Parameters.AddWithValue("@ReminderDate", GetValueOrDBNull(entity.ReminderDate));
            command.Parameters.AddWithValue("@Title", GetValueOrDBNull(entity.Title));
        }

        public Event Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "EventSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

        public List<Event> SelectList(Guid masterGuid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [Event] WITH (READUNCOMMITTED) WHERE [MasterGuid] = @MasterGuid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            return SelectList(command);
        }

    }
}

