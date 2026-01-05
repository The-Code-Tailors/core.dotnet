using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class UserDomain
    {
        public static UserDomain Select(Milieu milieu, long domainId, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserDomainSelect);
            UserDomain domain = new UserDomain();
            UserDomainSqlController controller = (UserDomainSqlController)domain.GetDefaultController();
            return controller.Select(domainId, userId);
        }

        public static List<UserDomain> SelectListByDomainId(Milieu milieu, long domainId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserDomainSelect);
            UserDomain domain = new UserDomain();
            UserDomainSqlController controller = (UserDomainSqlController)domain.GetDefaultController();
            return controller.SelectListByDomainId(domainId);
        }

        public static List<UserDomain> SelectListByUserId(Milieu milieu, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.UserDomainSelect);
            UserDomain domain = new UserDomain();
            UserDomainSqlController controller = (UserDomainSqlController)domain.GetDefaultController();
            return controller.SelectListByUserId(userId);
        }

    }
}

