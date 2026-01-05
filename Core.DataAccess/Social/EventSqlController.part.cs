using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class EventSqlController
    {
        public List<Event> SelectList(Guid masterGuid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [Event] WHERE [MasterGuid] = @MasterGuid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            return SelectList(command);
        }

    }
}

