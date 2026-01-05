using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityColumn : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.FlexibleEntityColumnDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.FlexibleEntityColumnUpdate; } }

        public long FlexibleColumnId { get; set; }
        public long FlexibleEntityId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            FlexibleEntityColumnSqlController controller = new FlexibleEntityColumnSqlController(configuration, this);
            return controller;
        }

        public static FlexibleEntityColumn Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnSelect);
            FlexibleEntityColumnSqlController controller = (FlexibleEntityColumnSqlController)new FlexibleEntityColumn().GetDefaultController();
            return controller.Select(id);
        }

        public static FlexibleEntityColumn Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnSelect);
            FlexibleEntityColumnSqlController controller = (FlexibleEntityColumnSqlController)new FlexibleEntityColumn().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<FlexibleEntityColumn> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnSelect);
            FlexibleEntityColumnSqlController controller = (FlexibleEntityColumnSqlController)new FlexibleEntityColumn().GetDefaultController();
            return controller.SelectList();
        }

        public List<FlexibleEntityColumn> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleEntityColumnSqlController controller = (FlexibleEntityColumnSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public FlexibleEntityColumn SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleEntityColumnSqlController controller = (FlexibleEntityColumnSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

