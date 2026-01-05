using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess
{
    public partial class Issue : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.IssueDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.IssueUpdate; } }


        protected override Controller GetController(ControllerConfiguration configuration)
        {
            IssueSqlController controller = new IssueSqlController(configuration, this);
            return controller;
        }

        public static Issue Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.IssueSelect);
            IssueSqlController controller = (IssueSqlController)new Issue().GetDefaultController();
            return controller.Select(id);
        }

        public static Issue Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.IssueSelect);
            IssueSqlController controller = (IssueSqlController)new Issue().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<Issue> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.IssueSelect);
            IssueSqlController controller = (IssueSqlController)new Issue().GetDefaultController();
            return controller.SelectList();
        }

        public List<Issue> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.IssueSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            IssueSqlController controller = (IssueSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public Issue SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.IssueSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            IssueSqlController controller = (IssueSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

