using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleColumn
    {
        public string Type { get; set; }

        public override void Delete(Milieu milieu, Microsoft.Data.SqlClient.SqlTransaction transaction, bool permanently)
        {
            if (FlexibleEntityColumn.SelectList(milieu).Exists(x => x.FlexibleColumnId == Id))
            {
                DataAccessException e = new DataAccessException("You cannot delete this column because it is assigned to one or more entities!");
                e.Data.Add("Test", "FlexibleColumn-FlexibleEntityColumn");
                throw e;
            }
            if (FlexibleEntityColumnInstance.SelectList(milieu).Exists(x => x.FlexibleColumnId == Id))
            {
                DataAccessException e = new DataAccessException("You cannot delete this column because it is in use!");
                e.Data.Add("Test", "FlexibleColumn-FlexibleEntityColumnInstance");
                throw e;
            }
            base.Delete(milieu, transaction, permanently);
        }

        public static FlexibleColumn Select(Milieu milieu, string name)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleColumnSelect);
            FlexibleColumnSqlController controller = (FlexibleColumnSqlController)new FlexibleColumn().GetDefaultController();
            return controller.Select(name);
        }

        public static List<FlexibleColumn> SelectListByFlexibleEntityId(Milieu milieu, long flexibleEntityId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleColumnSelect);
            FlexibleColumnSqlController controller = (FlexibleColumnSqlController)new FlexibleColumn().GetDefaultController();
            return controller.SelectListByFlexibleEntityId(flexibleEntityId);
        }

        public static List<FlexibleColumn> SelectListByFlexibleEntityName(Milieu milieu, string flexibleEntityName)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleColumnSelect);
            FlexibleColumnSqlController controller = (FlexibleColumnSqlController)new FlexibleColumn().GetDefaultController();
            return controller.SelectListByFlexibleEntityName(flexibleEntityName);
        }

        public override void Update(Milieu milieu, SqlTransaction transaction)
        {
            if (Name == "Data" || Name == "DataType" || Name == "Guid")
            {
                throw new DataAccessException("Invalid flexible column name!");
            }

            base.Update(milieu, transaction);
        }

    }
}

