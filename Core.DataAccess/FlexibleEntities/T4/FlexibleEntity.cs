using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntity : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.FlexibleEntityDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.FlexibleEntityUpdate; } }

        public string Name { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            FlexibleEntitySqlController controller = new FlexibleEntitySqlController(configuration, this);
            return controller;
        }

        public static FlexibleEntity Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntitySelect);
            FlexibleEntitySqlController controller = (FlexibleEntitySqlController)new FlexibleEntity().GetDefaultController();
            return controller.Select(id);
        }

        public static FlexibleEntity Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntitySelect);
            FlexibleEntitySqlController controller = (FlexibleEntitySqlController)new FlexibleEntity().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<FlexibleEntity> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntitySelect);
            FlexibleEntitySqlController controller = (FlexibleEntitySqlController)new FlexibleEntity().GetDefaultController();
            return controller.SelectList();
        }

        public List<FlexibleEntity> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntitySelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleEntitySqlController controller = (FlexibleEntitySqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public FlexibleEntity SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntitySelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleEntitySqlController controller = (FlexibleEntitySqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

