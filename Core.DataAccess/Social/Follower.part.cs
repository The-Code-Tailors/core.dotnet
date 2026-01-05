using System;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Follower
    {
        public static Follower Select(Milieu milieu, Guid masterGuid, long userId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FollowerSelect);
            FollowerSqlController controller = (FollowerSqlController)new Follower().GetDefaultController();
            return controller.Select(masterGuid, userId);
        }

    }
}

