using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class RemarkUserSqlController
    {
        public RemarkUser Select(long remarkId, long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [RemarkUser] WHERE [RemarkId] = @RemarkId AND [UserId] = @UserId AND [DeleteDate] IS NULL AND [VersioningId] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@RemarkId", remarkId);
            command.Parameters.AddWithValue("@UserId", userId);
            return Select(command);
        }

        public List<RemarkUser> SelectListByRemarkId(long remarkId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [RemarkUser] WHERE [RemarkId] = @RemarkId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@RemarkId", remarkId);
            return SelectList(command);
        }

        public List<RemarkUser> SelectListByUserId(long userId)
        {
            SqlCommand command = new SqlCommand() { CommandText = "SELECT * FROM [RemarkUser] WHERE [UserId] = @UserId AND [DeleteDate] IS NULL", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@UserId", userId);
            return SelectList(command);
        }

    }
}

