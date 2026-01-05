using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Notification : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.NotificationDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.NotificationUpdate; } }

        public string MasterEntity { get; set; }
        public Guid MasterGuid { get; set; }
        public long MasterId { get; set; }
        public bool Read { get; set; }
        public string Text { get; set; }
        public string TypeName { get; set; }
        public long UserId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            NotificationSqlController controller = new NotificationSqlController(configuration, this);
            return controller;
        }

        public static Notification Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.NotificationSelect);
            NotificationSqlController controller = (NotificationSqlController)new Notification().GetDefaultController();
            return controller.Select(id);
        }

        public static Notification Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.NotificationSelect);
            NotificationSqlController controller = (NotificationSqlController)new Notification().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Notification> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.NotificationSelect);
            NotificationSqlController controller = (NotificationSqlController)new Notification().GetDefaultController();
            return controller.SelectList();
        }

        public static List<Notification> SelectList(Milieu milieu, Guid masterGuid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.NotificationSelect);
            NotificationSqlController controller = (NotificationSqlController)new Notification().GetDefaultController();
            return controller.SelectList(masterGuid);
        }

        public List<Notification> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.NotificationSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            NotificationSqlController controller = (NotificationSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Notification SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.NotificationSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            NotificationSqlController controller = (NotificationSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

