using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using System.Web;

namespace com.fabioscagliola.Core.DataAccess.LogEntries
{
    public class UserSignInLogEntry : LogEntry
    {
        public DateTime Date { get; set; }
        public string UserAgent { get; set; }
        public string UserHostAddress { get; set; }
        public string UserHostName { get; set; }
        public string[] UserLanguages { get; set; }

        /// <summary>
        /// Only initialized by the SelectList2(Milieu milieu) method! 
        /// </summary>
        public string UserFName { get; set; }

        /// <summary>
        /// Only initialized by the SelectList2(Milieu milieu) method! 
        /// </summary>
        public string UserLName { get; set; }

        /// <summary>
        /// Only initialized by the SelectList2(Milieu milieu) method! 
        /// </summary>
        public string UserUName { get; set; }

        public UserSignInLogEntry() { }

        public UserSignInLogEntry(User user)
        {
            MasterData masterData = new MasterData(user);

            MasterEntity = masterData.MasterEntity;
            MasterGuid = masterData.MasterGuid;
            MasterId = masterData.MasterId;

            Date = DateTime.Now;

            if (HttpContext.Current != null)
            {
                UserAgent = HttpContext.Current.Request.UserAgent;
                UserHostAddress = HttpContext.Current.Request.UserHostAddress;
                UserHostName = HttpContext.Current.Request.UserHostName;
                UserLanguages = HttpContext.Current.Request.UserLanguages;
            }
        }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            UserSignInLogEntrySqlController controller = new UserSignInLogEntrySqlController(configuration, this);
            return controller;
        }

        /// <summary>
        /// Returns the latest 1000 log entries of the specified kind, including the users' first name, last name, and username; 
        /// the optional <paramref name="allRecords"/> parameter allows returning all the log entries 
        /// </summary>
        /// <param name="milieu">The milieu</param>
        /// <param name="allRecords">An optional Boolean value indicating if (false, default) the latest 1000 log entries will be returned, or (true) all the log entries will be returned</param>
        public static List<UserSignInLogEntry> SelectList2(Milieu milieu, bool allRecords = false)
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            UserSignInLogEntrySqlController controller = (UserSignInLogEntrySqlController)new UserSignInLogEntry().GetDefaultController();
            return controller.SelectList<UserSignInLogEntry>(allRecords);
        }

        /// <summary>
        /// Returns the latest 1000 log entries of the specified kind, belonging to the specified user, including the users' first name, last name, and username; 
        /// the optional <paramref name="allRecords"/> parameter allows returning all the log entries 
        /// </summary>
        /// <param name="milieu">The milieu</param>
        /// <param name="userId">The identifier of the user</param>
        /// <param name="allRecords">An optional Boolean value indicating if (false, default) the latest 1000 log entries will be returned, or (true) all the log entries will be returned</param>
        public static List<UserSignInLogEntry> SelectListByUserId(Milieu milieu, long userId, bool allRecords = false)
        {
            EnsureAuthorized(milieu, DataAccessFunction.LogEntrySelect);
            UserSignInLogEntrySqlController controller = (UserSignInLogEntrySqlController)new UserSignInLogEntry().GetDefaultController();
            return controller.SelectListByUserId<UserSignInLogEntry>(userId, allRecords);
        }

    }
}

