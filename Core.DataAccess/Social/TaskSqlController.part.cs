using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class TaskSqlController : DataAccessSqlController<Task>
    {
        public List<Task> SelectListByComplete(Guid masterGuid, bool complete)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [Task] WHERE [MasterGuid] = @MasterGuid AND Complete = @Complete AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            command.Parameters.AddWithValue("@Complete", complete);
            return SelectList(command);
        }

    }
}

