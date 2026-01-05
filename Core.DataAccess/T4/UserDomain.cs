using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserDomain : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.UserDomainDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.UserDomainUpdate; } }

        public long DomainId { get; set; }
        public long UserId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            UserDomainSqlController controller = new UserDomainSqlController(configuration, this);
            return controller;
        }

        public static UserDomain Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserDomainSelect);
            UserDomainSqlController controller = (UserDomainSqlController)new UserDomain().GetDefaultController();
            return controller.Select(id);
        }

        public static UserDomain Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserDomainSelect);
            UserDomainSqlController controller = (UserDomainSqlController)new UserDomain().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<UserDomain> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserDomainSelect);
            UserDomainSqlController controller = (UserDomainSqlController)new UserDomain().GetDefaultController();
            return controller.SelectList();
        }

        public static List<UserDomain> SelectList(Milieu milieu, long domainId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserDomainSelect);
            UserDomainSqlController controller = (UserDomainSqlController)new UserDomain().GetDefaultController();
            return controller.SelectList(domainId);
        }

        public List<UserDomain> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserDomainSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            UserDomainSqlController controller = (UserDomainSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public UserDomain SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserDomainSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            UserDomainSqlController controller = (UserDomainSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

