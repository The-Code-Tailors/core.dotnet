using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityColumnInstanceSqlController
    {
        public FlexibleEntityColumnInstance Select(long flexibleEntityInstanceId, long flexibleColumnId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [FlexibleEntityColumnInstance] WITH (READUNCOMMITTED) WHERE [FlexibleEntityInstanceId] = @FlexibleEntityInstanceId AND [FlexibleColumnId] = @FlexibleColumnId AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@FlexibleEntityInstanceId", flexibleEntityInstanceId);
            command.Parameters.AddWithValue("@FlexibleColumnId", flexibleColumnId);
            return Select(command);
        }

    }
}

