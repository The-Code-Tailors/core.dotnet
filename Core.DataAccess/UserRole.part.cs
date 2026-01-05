using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserRole
    {
        public static UserRole Select(Milieu milieu, long domainId, long userId, long roleId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserRoleSelect);
            UserRole user = new UserRole();
            UserRoleSqlController controller = (UserRoleSqlController)user.GetDefaultController();
            return controller.Select(domainId, userId, roleId);
        }

        public static List<UserRole> SelectListByUserId(Milieu milieu, long domainId, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserRoleSelect);
            UserRole user = new UserRole();
            UserRoleSqlController controller = (UserRoleSqlController)user.GetDefaultController();
            return controller.SelectListByUserId(domainId, userId);
        }

        public static List<UserRole> SelectListByRoleId(Milieu milieu, long domainId, long roleId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserRoleSelect);
            UserRole user = new UserRole();
            UserRoleSqlController controller = (UserRoleSqlController)user.GetDefaultController();
            return controller.SelectListByRoleId(domainId, roleId);
        }

    }
}

