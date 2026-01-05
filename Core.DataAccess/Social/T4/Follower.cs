using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Follower : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.FollowerDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.FollowerUpdate; } }

        public string MasterEntity { get; set; }
        public Guid MasterGuid { get; set; }
        public long MasterId { get; set; }
        public long UserId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            FollowerSqlController controller = new FollowerSqlController(configuration, this);
            return controller;
        }

        public static Follower Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FollowerSelect);
            FollowerSqlController controller = (FollowerSqlController)new Follower().GetDefaultController();
            return controller.Select(id);
        }

        public static Follower Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FollowerSelect);
            FollowerSqlController controller = (FollowerSqlController)new Follower().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Follower> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FollowerSelect);
            FollowerSqlController controller = (FollowerSqlController)new Follower().GetDefaultController();
            return controller.SelectList();
        }

        public static List<Follower> SelectList(Milieu milieu, Guid masterGuid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FollowerSelect);
            FollowerSqlController controller = (FollowerSqlController)new Follower().GetDefaultController();
            return controller.SelectList(masterGuid);
        }

        public List<Follower> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FollowerSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FollowerSqlController controller = (FollowerSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Follower SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FollowerSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FollowerSqlController controller = (FollowerSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

