using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class EventUser : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.EventUserDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.EventUserUpdate; } }

        public long EventId { get; set; }
        public long UserId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            EventUserSqlController controller = new EventUserSqlController(configuration, this);
            return controller;
        }

        public static EventUser Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUserSelect);
            EventUserSqlController controller = (EventUserSqlController)new EventUser().GetDefaultController();
            return controller.Select(id);
        }

        public static EventUser Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUserSelect);
            EventUserSqlController controller = (EventUserSqlController)new EventUser().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<EventUser> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUserSelect);
            EventUserSqlController controller = (EventUserSqlController)new EventUser().GetDefaultController();
            return controller.SelectList();
        }

        public List<EventUser> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUserSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            EventUserSqlController controller = (EventUserSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public EventUser SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUserSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            EventUserSqlController controller = (EventUserSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

