using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class RoleFunction
    {
        public static List<RoleFunction> SelectList(Milieu milieu, long roleId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleFunctionSelect);
            RoleFunction roleFunction = new RoleFunction();
            RoleFunctionSqlController controller = (RoleFunctionSqlController)roleFunction.GetDefaultController();
            return controller.SelectList(roleId);
        }

        public static RoleFunction Select(Milieu milieu, long roleId, DataAccessFunction function)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RoleFunctionSelect);
            RoleFunction roleFunction = new RoleFunction();
            RoleFunctionSqlController controller = (RoleFunctionSqlController)roleFunction.GetDefaultController();
            return controller.Select(roleId, function);
        }

    }
}

