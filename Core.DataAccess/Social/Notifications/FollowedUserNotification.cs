namespace com.fabioscagliola.Core.DataAccess.Social.Notifications
{
    /// <summary>
    /// Sent when users start being followed 
    /// </summary>
    public class FollowedUserNotification : UserNotification
    {
        public FollowedUserNotification() { }

        public FollowedUserNotification(User user, long userId) : base(user, string.Format("{0} started following you", user.FullName), userId) { }

    }
}

