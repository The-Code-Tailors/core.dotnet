using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class TaskUserSqlController : DataAccessSqlController<TaskUser>
    {
        public TaskUserSqlController(ControllerConfiguration configuration, TaskUser entity)
            : base(configuration, entity, "TaskUserDelete", "TaskUserInsert", "TaskUserSelect", "TaskUserSelectList", "TaskUserSelectVersionHistory", "TaskUserSelectVersionHistoryItem", "TaskUserUpdate")
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
            command.Parameters.AddWithValue("@TaskId", GetValueOrDBNull(entity.TaskId));
            command.Parameters.AddWithValue("@UserId", GetValueOrDBNull(entity.UserId));
        }

        public TaskUser Select(Guid guid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "TaskUserSelectByGuid", CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Guid", guid);
            return Select(command);
        }

    }
}

