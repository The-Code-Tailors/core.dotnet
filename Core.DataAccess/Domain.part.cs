using com.fabioscagliola.Core.DataAccess.Social;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class Domain
    {
        private static List<Domain> data;

        private static void ReloadData()
        {
            data = Domain.SelectList1(Milieu.SystemMilieu)
                .OrderBy(x => x.Name).ToList();
        }

        static Domain()
        {
            ReloadData();
        }

        public static Domain Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainSelect);
            return data.Find(x => x.Id == id);
        }

        public static Domain Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainSelect);
            return data.Find(x => x.Guid == guid);
        }

        public static List<Domain> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainSelect);
            return data;
        }

        public override void Delete(Milieu milieu, SqlTransaction transaction, bool permanently)
        {
            base.Delete(milieu, transaction, permanently);

            ReloadData();
        }

        public override void Update(Milieu milieu, SqlTransaction transaction)
        {
            base.Update(milieu, transaction);

            ReloadData();
        }

        #region User list management

        protected List<User> userList = null;

        public void AssignUser(Milieu milieu, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainUpdate);
            UserDomain userDomain = UserDomain.Select(milieu, this.Id, user.Id);
            if (userDomain.Id == 0)  // Ensure it does not already exist 
            {
                userDomain = new UserDomain() { DomainId = this.Id, UserId = user.Id, };
                userDomain.Update(milieu);
                userList = null;  // Flush cache 
            }
        }

        public void RemoveUser(Milieu milieu, User user)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainUpdate);
            UserDomain userDomain = UserDomain.Select(milieu, this.Id, user.Id);
            if (userDomain.Id != 0)  // Ensure it exists 
            {
                userDomain.Delete(milieu);
                userList = null;  // Flush cache 
            }
        }

        public List<User> SelectUserList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.DomainSelect);
            if (userList == null)
            {
                userList = new List<User>();
                List<UserDomain> userDomainList = UserDomain.SelectListByDomainId(milieu, this.Id);
                foreach (UserDomain userDomain in userDomainList)
                {
                    User user = User.Select(milieu, userDomain.UserId);
                    userList.Add(user);
                }
            }
            return userList;
        }

        #endregion

        #region Icon

        public static int IconWidth { get { return 24; } }
        public static int IconHeight { get { return 24; } }

        protected Blob icon;

        public Guid IconGuid { get; set; }

        public Blob Icon
        {
            get
            {
                if (IconGuid != Guid.Empty && icon == null)
                {
                    icon = Blob.Select(Milieu.SystemMilieu, IconGuid);
                }
                return icon;
            }
        }

        #endregion

    }
}

