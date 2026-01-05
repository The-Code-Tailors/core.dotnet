using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserRole : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.UserRoleDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.UserRoleUpdate; } }

        public long? DomainId { get; set; }
        public long RoleId { get; set; }
        public long UserId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            UserRoleSqlController controller = new UserRoleSqlController(configuration, this);
            return controller;
        }

        public static UserRole Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserRoleSelect);
            UserRoleSqlController controller = (UserRoleSqlController)new UserRole().GetDefaultController();
            return controller.Select(id);
        }

        public static UserRole Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserRoleSelect);
            UserRoleSqlController controller = (UserRoleSqlController)new UserRole().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<UserRole> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserRoleSelect);
            UserRoleSqlController controller = (UserRoleSqlController)new UserRole().GetDefaultController();
            return controller.SelectList();
        }

        public static List<UserRole> SelectList(Milieu milieu, long domainId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserRoleSelect);
            UserRoleSqlController controller = (UserRoleSqlController)new UserRole().GetDefaultController();
            return controller.SelectList(domainId);
        }

        public List<UserRole> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserRoleSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            UserRoleSqlController controller = (UserRoleSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public UserRole SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserRoleSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            UserRoleSqlController controller = (UserRoleSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

