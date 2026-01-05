using com.fabioscagliola.Core.Data;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntityInstance
    {
        public override void Delete(Milieu milieu, SqlTransaction transaction, bool permanently)
        {
            foreach (FlexibleEntityColumnInstance flexibleEntityColumnInstance in FlexibleEntityColumnInstance.SelectList(milieu).FindAll(x => x.FlexibleEntityInstanceId == Id))
            {
                flexibleEntityColumnInstance.Delete(milieu, transaction, permanently);
            }
            base.Delete(milieu, transaction, permanently);
        }

        public object GetValue(Milieu milieu, long flexibleColumnId)
        {
            object value = null;
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnInstanceSelect);
            FlexibleColumn flexibleColumn = FlexibleColumn.Select(milieu, flexibleColumnId);
            FlexibleEntityColumnInstance flexibleEntityColumnInstance = FlexibleEntityColumnInstance.Select(milieu, Id, flexibleColumnId);
            if (flexibleEntityColumnInstance.XmlValue != null)
            {
                value = Util.Deserialize(flexibleEntityColumnInstance.XmlValue, Type.GetType(flexibleColumn.Type));
            }
            return value;
        }

        public void SetValue(Milieu milieu, long flexibleColumnId, object value)
        {
            SetValue(milieu, flexibleColumnId, value, null);
        }

        public void SetValue(Milieu milieu, long flexibleColumnId, object value, SqlTransaction transaction)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityColumnInstanceUpdate);
            FlexibleEntityColumnInstance flexibleEntityColumnInstance = FlexibleEntityColumnInstance.Select(milieu, Id, flexibleColumnId);
            if (flexibleEntityColumnInstance.Id == 0)
            {
                flexibleEntityColumnInstance = new FlexibleEntityColumnInstance();
                flexibleEntityColumnInstance.FlexibleColumnId = flexibleColumnId;
                flexibleEntityColumnInstance.FlexibleEntityInstanceId = Id;
            }
            if (value == null)
            {
                flexibleEntityColumnInstance.XmlValue = null;
            }
            else
            {
                flexibleEntityColumnInstance.XmlValue = Util.Serialize(value);
            }
            flexibleEntityColumnInstance.Update(milieu, transaction);
        }

        public static List<FlexibleEntityInstance> SelectList(Milieu milieu, long flexibleEntityId)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityInstanceSelect);
            FlexibleEntityInstanceSqlController controller = (FlexibleEntityInstanceSqlController)new FlexibleEntityInstance().GetDefaultController();
            return controller.SelectListByFlexibleEntityId(flexibleEntityId);
        }

    }
}

