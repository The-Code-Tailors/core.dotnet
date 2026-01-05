using System.Collections.Generic;
using System.Linq;

namespace com.fabioscagliola.Core.DataAccess.FlexibleEntities
{
    public partial class FlexibleEntity
    {
        protected List<FlexibleColumn> flexibleColumnList;

        public override void Delete(Milieu milieu, System.Data.SqlClient.SqlTransaction transaction, bool permanently)
        {
            if (FlexibleEntityColumn.SelectList(milieu).Exists(x => x.FlexibleEntityId == Id))
            {
                DataAccessException e = new DataAccessException("You cannot delete this entity because it is assigned to one or more columns!");
                e.Data.Add("Test", "FlexibleEntity-FlexibleEntityColumn");
                throw e;
            }
            if (FlexibleEntityInstance.SelectList(milieu).Exists(x => x.FlexibleEntityId == Id))
            {
                DataAccessException e = new DataAccessException("You cannot delete this entity because it is in use!");
                e.Data.Add("Test", "FlexibleEntity-FlexibleEntityInstance");
                throw e;
            }
            base.Delete(milieu, transaction, permanently);
        }

        public static FlexibleEntity Select(Milieu milieu, string name)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntitySelect);
            FlexibleEntitySqlController controller = (FlexibleEntitySqlController)new FlexibleEntity().GetDefaultController();
            return controller.Select(name);
        }

        #region FlexibleColumn management

        public void AssignFlexibleColumn(Milieu milieu, FlexibleColumn column)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityUpdate);
            FlexibleEntityColumn flexibleEntityColumn = FlexibleEntityColumn.Select(milieu, column.Id, this.Id);
            if (flexibleEntityColumn.Id == 0)  // Ensure it does not already exist 
            {
                List<FlexibleEntityColumn> flexibleEntityColumnList = FlexibleEntityColumn.SelectListByFlexibleEntityId(milieu, this.Id);

                flexibleEntityColumn = new FlexibleEntityColumn() { FlexibleColumnId = column.Id, FlexibleEntityId = this.Id, };
                flexibleEntityColumn.SequenceNumber = (flexibleEntityColumnList.Count > 0) ? flexibleEntityColumnList.Max(x => x.SequenceNumber) + 1 : 0;
                flexibleEntityColumn.Update(milieu);
                flexibleColumnList = null;  // Flush cache 
            }
        }

        public void RemoveFlexibleColumn(Milieu milieu, FlexibleColumn column)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityUpdate);
            if (FlexibleEntityInstance.SelectList(milieu).Exists(x => x.FlexibleEntityId == Id))
            {
                DataAccessException e = new DataAccessException("You cannot remove this column because it is in use!");
                e.Data.Add("Test", "RemoveFlexibleColumn");
                throw e;
            }
            FlexibleEntityColumn flexibleEntityColumn = FlexibleEntityColumn.Select(milieu, column.Id, this.Id);
            if (flexibleEntityColumn.Id != 0)  // Ensure it exists 
            {
                flexibleEntityColumn.Delete(milieu);
                flexibleColumnList = null;  // Flush cache 
            }
        }

        public List<FlexibleColumn> SelectFlexibleColumnList(Milieu milieu)
        {
            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntitySelect);
            if (flexibleColumnList == null)
            {
                flexibleColumnList = new List<FlexibleColumn>();
                List<FlexibleEntityColumn> flexibleEntityColumnList = FlexibleEntityColumn.SelectListByFlexibleEntityId(milieu, this.Id);
                flexibleEntityColumnList.Sort((a, b) => a.SequenceNumber.CompareTo(b.SequenceNumber));
                foreach (FlexibleEntityColumn flexibleEntityColumn in flexibleEntityColumnList)
                {
                    FlexibleColumn flexibleColumn = FlexibleColumn.Select(milieu, flexibleEntityColumn.FlexibleColumnId);
                    flexibleColumnList.Add(flexibleColumn);
                }
            }
            return flexibleColumnList;
        }

        /// <summary>
        /// Ensures that flexible columns' sequence numbers start from zero and are contiguous 
        /// </summary>
        public void DoFlexibleColumnListSequenceNumber(Milieu milieu)
        {
            // TODO: Transaction 

            EnsureAuthorized(milieu, DataAccessFunction.FlexibleEntityUpdate);
            List<FlexibleEntityColumn> flexibleEntityColumnList = FlexibleEntityColumn.SelectListByFlexibleEntityId(milieu, this.Id);
            flexibleEntityColumnList.Sort((a, b) => a.SequenceNumber.CompareTo(b.SequenceNumber));
            for (int i = 0; i < flexibleEntityColumnList.Count; i++)
            {
                FlexibleEntityColumn flexibleEntityColumn = flexibleEntityColumnList[i];
                flexibleEntityColumn.SequenceNumber = i;
                flexibleEntityColumn.Update(milieu);
            }
        }

        #endregion

    }
}

