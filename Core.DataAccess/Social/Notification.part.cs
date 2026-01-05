using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Notification
    {
        public long EntityInsertUserId { get; set; }

        public Notification() { }

        public Notification(string text, long userId)
        {
            Text = text;
            TypeName = GetType().FullName;
            UserId = userId;
        }

        public Notification(DataAccessEntity masterEntity, string text, long userId)
            : this(text, userId)
        {
            MasterEntity = masterEntity.GetType().FullName;
            MasterGuid = masterEntity.Guid;
            MasterId = masterEntity.Id;
            EntityInsertUserId = masterEntity.InsertUserId.Value;
        }

        protected User entityInsertUserId;

        public User EntityInsertUser
        {
            get
            {
                if (entityInsertUserId == null && EntityInsertUserId != 0)
                {
                    entityInsertUserId = User.Select(Milieu.SystemMilieu, EntityInsertUserId);
                }
                return entityInsertUserId;
            }
        }

        public static List<Notification> SelectList(Milieu milieu, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.NotificationSelect);
            NotificationSqlController controller = (NotificationSqlController)new Notification().GetDefaultController();
            return controller.SelectList(userId);
        }

        /// <summary>
        /// This method creates a notification for each event assigned to the specified user with a reminder set in the past 
        /// </summary>
        public static void DoEventUserNotifications(long userId)
        {
            List<Event> eventList = Event.SelectList(Milieu.SystemMilieu, userId);

            foreach (Event e in eventList)
            {
                if (e.ReminderDate.HasValue && DateTime.Now > e.ReminderDate)
                {
                    List<Notification> notificationList = Notification.SelectList(Milieu.SystemMilieu, userId);

                    bool exists = false;

                    foreach (Notification notification in notificationList)
                    {
                        EventUserNotification eventUserNotification = notification as EventUserNotification;

                        if (eventUserNotification != null && eventUserNotification.EventId == e.Id)
                        {
                            exists = true;
                            break;
                        }
                    }

                    if (!exists)
                    {
                        EventUserNotification notification = new EventUserNotification();

                        notification.EventId = e.Id;

                        notification.MasterEntity = e.MasterEntity;
                        notification.MasterGuid = e.MasterGuid;
                        notification.MasterId = e.MasterId;
                        notification.EntityInsertUserId = e.InsertUserId.Value;
                        notification.Text = e.Title;
                        notification.TypeName = typeof(EventUserNotification).ToString();
                        notification.UserId = userId;
                        notification.Update(Milieu.SystemMilieu);
                    }
                }
            }
        }

    }
}

