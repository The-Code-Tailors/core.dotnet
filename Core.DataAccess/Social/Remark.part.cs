using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Remark
    {
        protected List<User> userList = null;

        public static List<Remark> SelectList(Milieu milieu, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkSelect);
            List<Remark> remarkList = new List<Remark>();
            List<RemarkUser> remarkUserList = RemarkUser.SelectListByUserId(milieu, userId);
            foreach (RemarkUser remarkUser in remarkUserList)
            {
                Remark remark = Remark.Select(milieu, remarkUser.RemarkId);

                if (remark.Id != 0)  // TODO: [Fabio] Delete RemarkUser records when deleting Remark records 
                {
                    remarkList.Add(remark);
                }
            }
            return remarkList;
        }

        #region User list management

        public void AssignUser(Milieu milieu, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUpdate);
            RemarkUser remarkUser = RemarkUser.Select(milieu, this.Id, user.Id);
            if (remarkUser.Id == 0)  // Ensure it does not already exist 
            {
                remarkUser = new RemarkUser() { RemarkId = this.Id, UserId = user.Id, };
                remarkUser.Update(milieu);
                userList = null;  // Flush cache 

                RemarkUserNotification notification = new RemarkUserNotification();
                notification.MasterEntity = this.MasterEntity;
                notification.MasterGuid = this.MasterGuid;
                notification.MasterId = this.MasterId;
                notification.EntityInsertUserId = milieu.UserId;
                notification.Text = string.Format("{0} mentioned you in a remark.", User.Select(Milieu.SystemMilieu, milieu.UserId).FullName);  // TODO: RESX 
                notification.TypeName = typeof(RemarkUserNotification).ToString();
                notification.UserId = user.Id;
                notification.Update(Milieu.SystemMilieu);

            }
        }

        public void RemoveUser(Milieu milieu, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUpdate);
            RemarkUser remarkUser = RemarkUser.Select(milieu, this.Id, user.Id);
            if (remarkUser.Id != 0)  // Ensure it exists 
            {
                remarkUser.Delete(milieu);
                userList = null;  // Flush cache 
            }
        }

        public List<User> SelectUserList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkSelect);
            if (userList == null)
            {
                userList = new List<User>();
                List<RemarkUser> remarkUserList = RemarkUser.SelectListByRemarkId(milieu, this.Id);
                foreach (RemarkUser remarkUser in remarkUserList)
                {
                    User user = User.Select(milieu, remarkUser.UserId);
                    userList.Add(user);
                }
            }
            return userList;
        }

        #endregion

    }
}

