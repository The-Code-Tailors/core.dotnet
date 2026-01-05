using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class NotificationSqlController
    {
        public List<Notification> SelectList(long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [Notification] WHERE [UserId] = @UserId AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@UserId", userId);
            return SelectList(command);
        }

    }
}
