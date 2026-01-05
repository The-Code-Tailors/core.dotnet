using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntitySqlController
    {
        public FlexibleEntity Select(string name)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [FlexibleEntity] WHERE [Name] = @Name AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@Name", name);
            return Select(command);
        }

    }
}

