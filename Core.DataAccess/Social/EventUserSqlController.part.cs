using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class EventUserSqlController
    {
        public EventUser Select(long eventId, long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [EventUser] WHERE [EventId] = @EventId AND [UserId] = @UserId AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@EventId", eventId);
            command.Parameters.AddWithValue("@UserId", userId);
            return Select(command);
        }

        public List<EventUser> SelectListByEventId(long eventId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [EventUser] WHERE [EventId] = @EventId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@EventId", eventId);
            return SelectList(command);
        }

        public List<EventUser> SelectListByUserId(long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [EventUser] WHERE [UserId] = @UserId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@UserId", userId);
            return SelectList(command);
        }

    }
}

