using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityInstanceSqlController
    {
        public List<FlexibleEntityInstance> SelectListByFlexibleEntityId(long flexibleEntityId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [FlexibleEntityInstance] WHERE [FlexibleEntityId] = @FlexibleEntityId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@FlexibleEntityId", flexibleEntityId);
            return SelectList(command);
        }

    }
}

