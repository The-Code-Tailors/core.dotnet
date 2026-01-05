using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Event : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.EventDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.EventUpdate; } }

        public DateTime BeginDate { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public string MasterEntity { get; set; }
        public Guid MasterGuid { get; set; }
        public long MasterId { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string Title { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            EventSqlController controller = new EventSqlController(configuration, this);
            return controller;
        }

        public static Event Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventSelect);
            EventSqlController controller = (EventSqlController)new Event().GetDefaultController();
            return controller.Select(id);
        }

        public static Event Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventSelect);
            EventSqlController controller = (EventSqlController)new Event().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Event> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventSelect);
            EventSqlController controller = (EventSqlController)new Event().GetDefaultController();
            return controller.SelectList();
        }

        public static List<Event> SelectList(Milieu milieu, Guid masterGuid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventSelect);
            EventSqlController controller = (EventSqlController)new Event().GetDefaultController();
            return controller.SelectList(masterGuid);
        }

        public List<Event> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            EventSqlController controller = (EventSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Event SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            EventSqlController controller = (EventSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

