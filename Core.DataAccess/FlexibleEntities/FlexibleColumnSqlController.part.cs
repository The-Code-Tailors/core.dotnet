using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleColumnSqlController
    {
        public FlexibleColumn Select(string name)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [FlexibleColumn] WHERE [Name] = @Name AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@Name", name);
            return Select(command);
        }

        public List<FlexibleColumn> SelectListByFlexibleEntityId(long flexibleEntityId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT [FlexibleColumn].* FROM [FlexibleColumn] JOIN [FlexibleEntityColumn] ON [FlexibleColumn].[Id] = [FlexibleEntityColumn].[FlexibleColumnId] WHERE [FlexibleEntityColumn].[FlexibleEntityId] = @FlexibleEntityId AND [FlexibleColumn].[DeleteDate] IS NULL AND [FlexibleColumn].[VersioningId] IS NULL AND [FlexibleEntityColumn].[DeleteDate] IS NULL AND [FlexibleEntityColumn].[VersioningId] IS NULL ORDER BY [FlexibleEntityColumn].Data.query('/FlexibleEntityColumn/SequenceNumber').value('.', 'int')", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@FlexibleEntityId", flexibleEntityId);
            return SelectList(command);
        }

        public List<FlexibleColumn> SelectListByFlexibleEntityName(string flexibleEntityName)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT [FlexibleColumn].* FROM [FlexibleColumn] JOIN [FlexibleEntityColumn] ON [FlexibleColumn].[Id] = [FlexibleEntityColumn].[FlexibleColumnId] JOIN [FlexibleEntity] ON [FlexibleEntityColumn].[FlexibleEntityId] = [FlexibleEntity].[Id] WHERE [FlexibleEntity].[Name] = @FlexibleEntityName AND [FlexibleColumn].[DeleteDate] IS NULL AND [FlexibleColumn].[VersioningId] IS NULL AND [FlexibleEntityColumn].[DeleteDate] IS NULL AND [FlexibleEntityColumn].[VersioningId] IS NULL AND [FlexibleEntity].[DeleteDate] IS NULL AND [FlexibleEntity].[VersioningId] IS NULL ORDER BY [FlexibleEntityColumn].Data.query('/FlexibleEntityColumn/SequenceNumber').value('.', 'int')", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@FlexibleEntityName", flexibleEntityName);
            return SelectList(command);
        }

    }
}

