using System.Collections.Generic;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityColumn
    {
        public int SequenceNumber { get; set; }


        private FlexibleColumn _flexibleColumn;

        public FlexibleColumn FlexibleColumn
        {
            get
            {
                if (_flexibleColumn == null)
                {
                    _flexibleColumn = FlexibleColumn.Select(Milieu.SystemMilieu, FlexibleColumnId);
                }
                return _flexibleColumn;
            }
        }


        public static FlexibleEntityColumn Select(Milieu milieu, long flexibleColumnId, long flexibleEntityId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnSelect);
            FlexibleEntityColumnSqlController controller = (FlexibleEntityColumnSqlController)new FlexibleEntityColumn().GetDefaultController();
            return controller.Select(flexibleColumnId, flexibleEntityId);
        }

        public static List<FlexibleEntityColumn> SelectListByFlexibleEntityId(Milieu milieu, long flexibleEntityId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnSelect);
            FlexibleEntityColumnSqlController controller = (FlexibleEntityColumnSqlController)new FlexibleEntityColumn().GetDefaultController();
            return controller.SelectListByFlexibleEntityId(flexibleEntityId);
        }

    }
}

