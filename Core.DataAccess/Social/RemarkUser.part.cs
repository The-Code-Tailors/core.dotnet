using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class RemarkUser
    {
        public static RemarkUser Select(Milieu milieu, long remarkId, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUserSelect);
            RemarkUser entity = new RemarkUser();
            RemarkUserSqlController controller = (RemarkUserSqlController)entity.GetDefaultController();
            return controller.Select(remarkId, userId);
        }

        public static List<RemarkUser> SelectListByRemarkId(Milieu milieu, long remarkId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUserSelect);
            RemarkUser remarkUser = new RemarkUser();
            RemarkUserSqlController controller = (RemarkUserSqlController)remarkUser.GetDefaultController();
            return controller.SelectListByRemarkId(remarkId);
        }

        public static List<RemarkUser> SelectListByUserId(Milieu milieu, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUserSelect);
            RemarkUser remarkUser = new RemarkUser();
            RemarkUserSqlController controller = (RemarkUserSqlController)remarkUser.GetDefaultController();
            return controller.SelectListByUserId(userId);
        }

    }
}

