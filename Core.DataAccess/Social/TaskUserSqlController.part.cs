using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class TaskUserSqlController
    {
        public TaskUser Select(long taskId, long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [TaskUser] WHERE [TaskId] = @TaskId AND [UserId] = @UserId AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.Parameters.AddWithValue("@UserId", userId);
            return Select(command);
        }

        public List<TaskUser> SelectListByTaskId(long taskId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [TaskUser] WHERE [TaskId] = @TaskId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@TaskId", taskId);
            return SelectList(command);
        }

        public List<TaskUser> SelectListByUserId(long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [TaskUser] WHERE [UserId] = @UserId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@UserId", userId);
            return SelectList(command);
        }

    }
}

