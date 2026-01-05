using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class RemarkUser : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.RemarkUserDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.RemarkUserUpdate; } }

        public long RemarkId { get; set; }
        public long UserId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            RemarkUserSqlController controller = new RemarkUserSqlController(configuration, this);
            return controller;
        }

        public static RemarkUser Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUserSelect);
            RemarkUserSqlController controller = (RemarkUserSqlController)new RemarkUser().GetDefaultController();
            return controller.Select(id);
        }

        public static RemarkUser Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUserSelect);
            RemarkUserSqlController controller = (RemarkUserSqlController)new RemarkUser().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<RemarkUser> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUserSelect);
            RemarkUserSqlController controller = (RemarkUserSqlController)new RemarkUser().GetDefaultController();
            return controller.SelectList();
        }

        public List<RemarkUser> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUserSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            RemarkUserSqlController controller = (RemarkUserSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public RemarkUser SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkUserSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            RemarkUserSqlController controller = (RemarkUserSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

