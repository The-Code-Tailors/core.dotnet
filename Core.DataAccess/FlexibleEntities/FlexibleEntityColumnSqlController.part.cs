using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityColumnSqlController
    {
        public FlexibleEntityColumn Select(long flexibleColumnId, long flexibleEntityId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [FlexibleEntityColumn] WHERE [FlexibleColumnId] = @FlexibleColumnId  AND [FlexibleEntityId] = @FlexibleEntityId  AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@FlexibleColumnId", flexibleColumnId);
            command.Parameters.AddWithValue("@FlexibleEntityId", flexibleEntityId);
            return Select(command);
        }

        public List<FlexibleEntityColumn> SelectListByFlexibleEntityId(long flexibleEntityId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [FlexibleEntityColumn] WHERE [FlexibleEntityId] = @FlexibleEntityId AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@FlexibleEntityId", flexibleEntityId);
            return SelectList(command);
        }

    }
}

