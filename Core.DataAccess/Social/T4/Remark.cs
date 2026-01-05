using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.Social
{
    public partial class Remark : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.RemarkDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.RemarkUpdate; } }

        public string MasterEntity { get; set; }
        public Guid MasterGuid { get; set; }
        public long MasterId { get; set; }
        public string Text { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            RemarkSqlController controller = new RemarkSqlController(configuration, this);
            return controller;
        }

        public static Remark Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkSelect);
            RemarkSqlController controller = (RemarkSqlController)new Remark().GetDefaultController();
            return controller.Select(id);
        }

        public static Remark Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkSelect);
            RemarkSqlController controller = (RemarkSqlController)new Remark().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Remark> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkSelect);
            RemarkSqlController controller = (RemarkSqlController)new Remark().GetDefaultController();
            return controller.SelectList();
        }

        public static List<Remark> SelectList(Milieu milieu, Guid masterGuid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkSelect);
            RemarkSqlController controller = (RemarkSqlController)new Remark().GetDefaultController();
            return controller.SelectList(masterGuid);
        }

        public List<Remark> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            RemarkSqlController controller = (RemarkSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Remark SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.RemarkSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            RemarkSqlController controller = (RemarkSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

