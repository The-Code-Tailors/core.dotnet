using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class RemarkSqlController
    {
        public List<Remark> SelectList(Guid masterGuid)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [Remark] WHERE [MasterGuid] = @MasterGuid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            return SelectList(command);
        }

    }
}

