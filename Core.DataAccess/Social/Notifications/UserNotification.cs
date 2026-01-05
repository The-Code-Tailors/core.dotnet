namespace com.fabioscagliola.Core.DataAccess.Social.Notifications
{
    /// <summary>
    /// Base class for user-related notifications 
    /// </summary>
    public abstract class UserNotification : Notification
    {
        public UserNotification() { }

        public UserNotification(User user, string text, long userId) : base(user, text, userId) { }

    }
}

