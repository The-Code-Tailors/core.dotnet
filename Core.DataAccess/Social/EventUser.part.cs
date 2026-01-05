using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class EventUser
    {
        public static EventUser Select(Milieu milieu, long eventId, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUserSelect);
            EventUserSqlController controller = (EventUserSqlController)new EventUser().GetDefaultController();
            return controller.Select(eventId, userId);
        }

        public static List<EventUser> SelectListByEventId(Milieu milieu, long eventId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUserSelect);
            EventUserSqlController controller = (EventUserSqlController)new EventUser().GetDefaultController();
            return controller.SelectListByEventId(eventId);
        }

        public static List<EventUser> SelectListByUserId(Milieu milieu, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUserSelect);
            EventUserSqlController controller = (EventUserSqlController)new EventUser().GetDefaultController();
            return controller.SelectListByUserId(userId);
        }

    }
}

