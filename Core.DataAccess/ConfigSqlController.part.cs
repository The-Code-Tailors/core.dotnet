using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class ConfigSqlController
    {
        public Config Select(string key)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [Config] WITH (READUNCOMMITTED) WHERE [Key] = @Key AND [VersioningId] IS NULL AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@Key", key);
            return Select(command);
        }

    }
}

