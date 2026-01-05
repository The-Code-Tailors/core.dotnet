using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class Role : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.RoleDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.RoleUpdate; } }

        public string Name { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            RoleSqlController controller = new RoleSqlController(configuration, this);
            return controller;
        }

        public static Role Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleSelect);
            RoleSqlController controller = (RoleSqlController)new Role().GetDefaultController();
            return controller.Select(id);
        }

        public static Role Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleSelect);
            RoleSqlController controller = (RoleSqlController)new Role().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Role> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleSelect);
            RoleSqlController controller = (RoleSqlController)new Role().GetDefaultController();
            return controller.SelectList();
        }

        public List<Role> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            RoleSqlController controller = (RoleSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Role SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            RoleSqlController controller = (RoleSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

