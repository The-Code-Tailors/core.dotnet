using com.fabioscagliola.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers.PropertyGridHelperPropertyRenderers
{
    public class LookupPropertyRenderer : PropertyRenderer
    {
        protected Milieu milieu;

        public LookupPropertyRenderer(PropertyGridHelperProperty property, Milieu milieu) : base(property)
        {
            this.milieu = milieu;
        }

        public override object ParseValue(string s)
        {
            EnsureParentType();

            object result;

            long.TryParse(s, out long longResult);

            result = longResult;

            if (property.Type.Name.StartsWith("Nullable") && longResult == 0)  // Nullable<Int64> identifiers happen 
            {
                result = null;
            }

            return result;
        }

        public override string RenderEditor(FormHelper formHelper)
        {
            EnsureParentType();

            IEnumerable<DataAccessEntity> parentList;

            MethodInfo methodInfo = property.ParentType.GetMethod("SelectList", new Type[] { typeof(Milieu), typeof(long), });

            if (methodInfo != null && milieu.DomainId != 0)
            {
                parentList = (IEnumerable<DataAccessEntity>)methodInfo.Invoke(null, new object[] { milieu, milieu.DomainId, });
            }
            else
            {
                methodInfo = property.ParentType.GetMethod("SelectList", new Type[] { typeof(Milieu), });
                parentList = (IEnumerable<DataAccessEntity>)methodInfo.Invoke(null, new object[] { milieu, });
            }

            parentList = parentList.OrderBy(x => x.ToString());

            List<SelectListItem> itemList = new List<SelectListItem>();

            itemList.Add(new SelectListItem() { Text = null, Value = null });

            foreach (DataAccessEntity parent in parentList)
            {
                SelectListItem item = new SelectListItem();

                item.Value = parent.Id.ToString();
                item.Text = parent.ToString();

                if (property.Value != null)  // Nullable<Int64> identifiers happen 
                {
                    item.Selected = parent.Id == (long)property.Value;
                }

                itemList.Add(item);
            }

            return formHelper.DoSelectHelper(property.Identifier, property.Label, false, itemList, property.Description);
        }

        public override string RenderViewer()
        {
            EnsureParentType();

            string result = null;

            if (property.Value != null)  // Nullable<Int64> identifiers happen 
            {
                MethodInfo methodInfo = property.ParentType.GetMethod("Select", new Type[] { typeof(Milieu), typeof(long), });

                DataAccessEntity parent = (DataAccessEntity)methodInfo.Invoke(null, new object[] { milieu, property.Value, });

                result = parent.ToString();
            }

            return result;
        }

        /// <summary>
        /// Ensures that a parent type was indicated for the property using the PropertyGridHelper attribute, 
        /// if so, then the property must be a foreign key, and I therefore can safely assume that its type is either Int64 or Nullable<Int64> 
        /// </summary>
        protected void EnsureParentType()
        {
            if (property.ParentType == null)
            {
                throw new ApplicationException("A parent type was not indicated for the property!");
            }
        }

    }
}

