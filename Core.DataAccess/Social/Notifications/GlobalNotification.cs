using com.fabioscagliola.Core.Data;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.Social.Notifications
{
    /// <summary>
    /// Sent to everyone 
    /// </summary>
    public class GlobalNotification : Notification
    {
        public string Url { get; set; }

        public GlobalNotification() { }

        public GlobalNotification(string text, string url, long userId)
            : base(text, userId)
        {
            Url = url;
        }

        public static void SendToEveryone(Milieu milieu, string text, string url)
        {
            SqlController controller = (SqlController)new GlobalNotification().GetDefaultController();
            SqlConnection connection = controller.GetConnection();
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                try
                {
                    foreach (User user in User.SelectList(milieu).FindAll(x => x.Id != Milieu.SystemMilieu.UserId))
                    {
                        GlobalNotification notification = new GlobalNotification(text, url, user.Id);
                        notification.Update(milieu, transaction);
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            finally
            {
                transaction.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }

    }
}

