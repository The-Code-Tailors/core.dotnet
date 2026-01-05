using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class Issue
    {
        public enum IssueKind
        {
            ChangeRequest = 1,
            Error = 2,
        }

        public IssueKind Kind { get; set; }
        public string Text { get; set; }
        public bool Complete { get; set; }

        public override void Update(Milieu milieu, SqlTransaction transaction)
        {
            bool isInsert = Id == 0;

            base.Update(milieu, transaction);

            if ((isInsert && InsertUserId != 2) || (!isInsert && UpdateUserId != 2))  // 2 is me 
            {
                IssueNotification notification = new IssueNotification();
                notification.MasterEntity = typeof(Issue).FullName;
                notification.MasterGuid = Guid;
                notification.MasterId = Id;
                notification.EntityInsertUserId = 1;
                if (isInsert)
                {
                    notification.Text = string.Format("{0} created an issue.", InsertUser.FullName);
                }
                else
                {
                    notification.Text = string.Format("{0} updated an issue.", UpdateUser.FullName);
                }
                notification.TypeName = typeof(IssueNotification).ToString();
                notification.UserId = 2;
                notification.Update(Milieu.SystemMilieu);
            }
        }

    }
}

