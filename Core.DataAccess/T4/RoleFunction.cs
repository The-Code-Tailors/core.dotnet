using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class RoleFunction : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.RoleFunctionDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.RoleFunctionUpdate; } }

        public DataAccessFunction Function { get; set; }
        public long RoleId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            RoleFunctionSqlController controller = new RoleFunctionSqlController(configuration, this);
            return controller;
        }

        public static RoleFunction Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleFunctionSelect);
            RoleFunctionSqlController controller = (RoleFunctionSqlController)new RoleFunction().GetDefaultController();
            return controller.Select(id);
        }

        public static RoleFunction Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleFunctionSelect);
            RoleFunctionSqlController controller = (RoleFunctionSqlController)new RoleFunction().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<RoleFunction> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleFunctionSelect);
            RoleFunctionSqlController controller = (RoleFunctionSqlController)new RoleFunction().GetDefaultController();
            return controller.SelectList();
        }

        public List<RoleFunction> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleFunctionSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            RoleFunctionSqlController controller = (RoleFunctionSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public RoleFunction SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleFunctionSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            RoleFunctionSqlController controller = (RoleFunctionSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

