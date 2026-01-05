using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class LogEntrySqlController
    {
        public List<T> SelectList<T>() where T : LogEntry, new()
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT TOP 1000 * FROM [LogEntry] WITH (READUNCOMMITTED) WHERE [DataType] = @DataType AND [DeleteDate] IS NULL AND [VersioningId] IS NULL ORDER BY [InsertDate] DESC", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DataType", new T().DoDataType());
            return SelectList(command).OfType<T>().ToList();
        }

        public List<T> SelectList<T>(Guid masterGuid) where T : LogEntry, new()
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT TOP 1000 * FROM [LogEntry] WITH (READUNCOMMITTED) WHERE [DataType] = @DataType AND [MasterGuid] = @MasterGuid AND [DeleteDate] IS NULL AND [VersioningId] IS NULL ORDER BY [InsertDate] DESC", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DataType", new T().DoDataType());
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            return SelectList(command).OfType<T>().ToList();
        }

    }
}

