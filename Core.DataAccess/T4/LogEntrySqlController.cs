using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class LogEntrySqlController : DataAccessSqlController<LogEntry>
    {
        public LogEntrySqlController(ControllerConfiguration configuration, LogEntry entity)
            : base(configuration, entity, "LogEntryDelete", "LogEntryInsert", "LogEntrySelect", "LogEntrySelectList", "LogEntrySelectVersionHistory", "LogEntrySelectVersionHistoryItem", "LogEntryUpdate")
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
            command.Parameters.AddWithValue("@MasterEntity", GetValueOrDBNull(entity.MasterEntity));
            command.Parameters.AddWithValue("@MasterGuid", GetValueOrDBNull(entity.MasterGuid));
            command.Parameters.AddWithValue("@MasterId", GetValueOrDBNull(entity.MasterId));
        }

        public LogEntry Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "LogEntrySelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

        public List<LogEntry> SelectList(Guid masterGuid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [LogEntry] WITH (READUNCOMMITTED) WHERE [MasterGuid] = @MasterGuid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            return SelectList(command);
        }

    }
}

