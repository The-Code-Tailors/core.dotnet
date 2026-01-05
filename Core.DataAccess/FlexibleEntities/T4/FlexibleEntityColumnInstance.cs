using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityColumnInstance : DataAccessEntity
    {
        protected override DataAccessFunction DeleteDataAccessFunction { get { return DataAccessFunction.FlexibleEntityColumnInstanceDelete; } }

        protected override DataAccessFunction UpdateDataAccessFunction { get { return DataAccessFunction.FlexibleEntityColumnInstanceUpdate; } }

        public long FlexibleColumnId { get; set; }
        public long FlexibleEntityInstanceId { get; set; }
        public string XmlValue { get; set; }

        protected override Controller GetController(ControllerConfiguration configuration)
        {
            FlexibleEntityColumnInstanceSqlController controller = new FlexibleEntityColumnInstanceSqlController(configuration, this);
            return controller;
        }

        public static FlexibleEntityColumnInstance Select(Milieu milieu, long id)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnInstanceSelect);
            FlexibleEntityColumnInstanceSqlController controller = (FlexibleEntityColumnInstanceSqlController)new FlexibleEntityColumnInstance().GetDefaultController();
            return controller.Select(id);
        }

        public static FlexibleEntityColumnInstance Select(Milieu milieu, Guid guid)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnInstanceSelect);
            FlexibleEntityColumnInstanceSqlController controller = (FlexibleEntityColumnInstanceSqlController)new FlexibleEntityColumnInstance().GetDefaultController();
            return controller.Select(guid);
        }

        public static List<FlexibleEntityColumnInstance> SelectList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnInstanceSelect);
            FlexibleEntityColumnInstanceSqlController controller = (FlexibleEntityColumnInstanceSqlController)new FlexibleEntityColumnInstance().GetDefaultController();
            return controller.SelectList();
        }

        public List<FlexibleEntityColumnInstance> SelectVersionHistory(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnInstanceSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleEntityColumnInstanceSqlController controller = (FlexibleEntityColumnInstanceSqlController)this.GetDefaultController();
            return controller.SelectVersionHistory();
        }

        public FlexibleEntityColumnInstance SelectVersionHistoryItem(Milieu milieu, long itemId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnInstanceSelect);
            EnsureAuthorized(milieu, DataAccessFunction.SelectVersionHistory);
            FlexibleEntityColumnInstanceSqlController controller = (FlexibleEntityColumnInstanceSqlController)this.GetDefaultController();
            return controller.SelectVersionHistoryItem(itemId);
        }

    }
}

