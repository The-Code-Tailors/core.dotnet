using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class User : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.UserDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.UserUpdate; } }

        public string FName { get; set; }
        public string LName { get; set; }
        public string PHash { get; set; }
        public string UName { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            UserSqlController controller = new UserSqlController(configuration, this);
            return controller;
        }

        public static User Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            return controller.Select(id);
        }

        public static User Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<User> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            UserSqlController controller = (UserSqlController)new User().GetDefaultController();
            return controller.SelectList();
        }

        public List<User> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            UserSqlController controller = (UserSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public User SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            UserSqlController controller = (UserSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

