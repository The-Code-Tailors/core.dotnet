using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityInstance : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.FlexibleEntityInstanceDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.FlexibleEntityInstanceUpdate; } }

        public long FlexibleEntityId { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            FlexibleEntityInstanceSqlController controller = new FlexibleEntityInstanceSqlController(configuration, this);
            return controller;
        }

        public static FlexibleEntityInstance Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityInstanceSelect);
            FlexibleEntityInstanceSqlController controller = (FlexibleEntityInstanceSqlController)new FlexibleEntityInstance().GetDefaultController();
            return controller.Select(id);
        }

        public static FlexibleEntityInstance Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityInstanceSelect);
            FlexibleEntityInstanceSqlController controller = (FlexibleEntityInstanceSqlController)new FlexibleEntityInstance().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<FlexibleEntityInstance> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityInstanceSelect);
            FlexibleEntityInstanceSqlController controller = (FlexibleEntityInstanceSqlController)new FlexibleEntityInstance().GetDefaultController();
            return controller.SelectList();
        }

        public List<FlexibleEntityInstance> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityInstanceSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleEntityInstanceSqlController controller = (FlexibleEntityInstanceSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public FlexibleEntityInstance SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityInstanceSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleEntityInstanceSqlController controller = (FlexibleEntityInstanceSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

