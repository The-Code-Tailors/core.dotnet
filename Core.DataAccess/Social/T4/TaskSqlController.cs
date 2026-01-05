using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class TaskSqlController : DataAccessSqlController<Task>
    {
        public TaskSqlController(ControllerConfiguration configuration, Task entity)
            : base(configuration, entity, "TaskDelete", "TaskInsert", "TaskSelect", "TaskSelectList", "TaskSelectVersionHistory", "TaskSelectVersionHistoryItem", "TaskUpdate")
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
            command.Parameters.AddWithValue("@Complete", GetValueOrDBNull(entity.Complete));
            command.Parameters.AddWithValue("@MasterEntity", GetValueOrDBNull(entity.MasterEntity));
            command.Parameters.AddWithValue("@MasterGuid", GetValueOrDBNull(entity.MasterGuid));
            command.Parameters.AddWithValue("@MasterId", GetValueOrDBNull(entity.MasterId));
            command.Parameters.AddWithValue("@Text", GetValueOrDBNull(entity.Text));
        }

        public Task Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "TaskSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

        public List<Task> SelectList(Guid masterGuid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [Task] WITH (READUNCOMMITTED) WHERE [MasterGuid] = @MasterGuid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            return SelectList(command);
        }

    }
}

