using com.fabioscagliola.Core.Data;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace com.fabioscagliola.Core.DataAccess.LogEntries
{
    public class UserSignInLogEntrySqlController : LogEntrySqlController
    {
        public UserSignInLogEntrySqlController(ControllerConfiguration configuration, LogEntry entity) : base(configuration, entity) { }

        /// <summary>
        /// Returns the latest 1000 log entries of the specified kind, including the users' first name, last name, and username; 
        /// the optional <paramref name="allRecords"/> parameter allows returning all the log entries 
        /// </summary>
        /// <typeparam name="T">The type of the log entries to be returned</typeparam>
        /// <param name="allRecords">An optional Boolean value indicating if (false, default) the latest 1000 log entries will be returned, or (true) all the log entries will be returned</param>
        public List<T> SelectList<T>(bool allRecords = false)
        {
            SqlCommand command = new SqlCommand() { CommandText = $"SELECT { (allRecords ? "" : "TOP 1000 ") }[LogEntry].*, [User].[Data].query('/User/FName').value('.', 'nvarchar(450)') [FName], [User].[Data].query('/User/LName').value('.', 'nvarchar(450)') [LName], [User].[UName] FROM [LogEntry] WITH (READUNCOMMITTED) JOIN [User] ON [LogEntry].[MasterId] = [User].[Id] WHERE [LogEntry].[DeleteDate] IS NULL AND [LogEntry].[VersioningId] IS NULL AND [LogEntry].[DataType] = @DataType ORDER BY [LogEntry].[InsertDate] DESC", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DataType", entity.DoDataType());
            return SelectList(command).Cast<T>().ToList();
        }

        /// <summary>
        /// Returns the latest 1000 log entries of the specified kind, belonging to the specified user, including the users' first name, last name, and username; 
        /// the optional <paramref name="allRecords"/> parameter allows returning all the log entries 
        /// </summary>
        /// <typeparam name="T">The type of the log entries to be returned</typeparam>
        /// <param name="userId">The identifier of the user</param>
        /// <param name="allRecords">An optional Boolean value indicating if (false, default) the latest 1000 log entries will be returned, or (true) all the log entries will be returned</param>
        public List<T> SelectListByUserId<T>(long userId, bool allRecords = false)
        {
            SqlCommand command = new SqlCommand() { CommandText = $"SELECT { (allRecords ? "" : "TOP 1000 ") }[LogEntry].*, [User].[Data].query('/User/FName').value('.', 'nvarchar(450)') [FName], [User].[Data].query('/User/LName').value('.', 'nvarchar(450)') [LName], [User].[UName] FROM [LogEntry] WITH (READUNCOMMITTED) JOIN [User] ON [LogEntry].[MasterId] = [User].[Id] WHERE [User].[Id] = @UserId AND [LogEntry].[DeleteDate] IS NULL AND [LogEntry].[VersioningId] IS NULL AND [LogEntry].[DataType] = @DataType ORDER BY [LogEntry].[InsertDate] DESC", CommandType = CommandType.Text };
            command.Parameters.AddWithValue("@DataType", entity.DoDataType());
            command.Parameters.AddWithValue("@UserId", userId);
            return SelectList(command).Cast<T>().ToList();
        }

        protected override void Read(SqlDataReader dataReader, ref LogEntry entity)
        {
            base.Read(dataReader, ref entity);

            if (dataReader.ContainsColumn("FName"))
            {
                UserSignInLogEntry userSignInLogEntry = (UserSignInLogEntry)entity;
                userSignInLogEntry.UserFName = dataReader.GetString(dataReader.GetOrdinal("FName"));
                userSignInLogEntry.UserLName = dataReader.GetString(dataReader.GetOrdinal("LName"));
                userSignInLogEntry.UserUName = dataReader.GetString(dataReader.GetOrdinal("UName"));
            }
        }

    }
}

