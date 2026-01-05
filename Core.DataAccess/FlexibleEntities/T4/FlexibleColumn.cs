using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleColumn : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.FlexibleColumnDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.FlexibleColumnUpdate; } }

        public string Name { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            FlexibleColumnSqlController controller = new FlexibleColumnSqlController(configuration, this);
            return controller;
        }

        public static FlexibleColumn Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleColumnSelect);
            FlexibleColumnSqlController controller = (FlexibleColumnSqlController)new FlexibleColumn().GetDefaultController();
            return controller.Select(id);
        }

        public static FlexibleColumn Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleColumnSelect);
            FlexibleColumnSqlController controller = (FlexibleColumnSqlController)new FlexibleColumn().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<FlexibleColumn> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleColumnSelect);
            FlexibleColumnSqlController controller = (FlexibleColumnSqlController)new FlexibleColumn().GetDefaultController();
            return controller.SelectList();
        }

        public List<FlexibleColumn> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleColumnSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleColumnSqlController controller = (FlexibleColumnSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public FlexibleColumn SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleColumnSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleColumnSqlController controller = (FlexibleColumnSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

