using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Event
    {
        protected List<User> userList = null;

        public static List<Event> SelectList(Milieu milieu, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventSelect);
            List<Event> eventList = new List<Event>();
            List<EventUser> eventUserList = EventUser.SelectListByUserId(milieu, userId);
            foreach (EventUser eventUser in eventUserList)
            {
                Event e = Event.Select(milieu, eventUser.EventId);

                if (e.Id != 0)  // TODO: [Fabio] Delete EventUser records when deleting Event records 
                {
                    eventList.Add(e);
                }
            }
            return eventList;
        }

        #region User list management

        public void AssignUser(Milieu milieu, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUpdate);
            EventUser eventUser = EventUser.Select(milieu, this.Id, user.Id);
            if (eventUser.Id == 0)  // Ensure it does not already exist 
            {
                eventUser = new EventUser() { EventId = this.Id, UserId = user.Id, };
                eventUser.Update(milieu);
                userList = null;  // Flush cache 
            }
        }

        public void RemoveUser(Milieu milieu, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventUpdate);
            EventUser eventUser = EventUser.Select(milieu, this.Id, user.Id);
            if (eventUser.Id != 0)  // Ensure it exists 
            {
                eventUser.Delete(milieu);
                userList = null;  // Flush cache 
            }
        }

        public List<User> SelectUserList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.EventSelect);
            if (userList == null)
            {
                userList = new List<User>();
                List<EventUser> eventUserList = EventUser.SelectListByEventId(milieu, this.Id);
                foreach (EventUser eventUser in eventUserList)
                {
                    User user = User.Select(milieu, eventUser.UserId);
                    userList.Add(user);
                }
            }
            return userList;
        }

        #endregion

    }
}

