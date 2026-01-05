using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class FollowerSqlController
    {
        public Follower Select(Guid masterGuid, long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [Follower] WHERE [MasterGuid] = @MasterGuid AND [UserId] = @UserId AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@MasterGuid", masterGuid);
            command.Parameters.AddWithValue("@UserId", userId);
            return Select(command);
        }

    }
}

