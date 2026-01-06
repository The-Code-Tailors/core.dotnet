using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers.PropertyGridHelperPropertyRenderers
{
    public class EnumPropertyRenderer : PropertyRenderer
    {
        public EnumPropertyRenderer(PropertyGridHelperProperty property) : base(property) { }

        public override object ParseValue(string s)
        {
            EnumConverter enumConverter = property.EnumConverter;

            return enumConverter.ConvertFrom(null, CultureInfo.InvariantCulture, s);
        }

        public override string RenderEditor(FormHelper formHelper)
        {
            EnumConverter enumConverter = property.EnumConverter;

            if (property.IsReadOnly)
            {
                string value = (string)enumConverter.ConvertTo(null, CultureInfo.InvariantCulture, property.Value, typeof(string));

                return formHelper.DoTextHelper(property.Identifier, property.Label, null, false, value, true, property.Description);
            }
            else
            {
                List<SelectListItem> itemList = new List<SelectListItem>();

                foreach (object ob in enumConverter.GetStandardValues())
                {
                    SelectListItem item = new SelectListItem();
                    item.Value = ob.ToString();
                    item.Text = (string)enumConverter.ConvertTo(null, CultureInfo.InvariantCulture, ob, typeof(string));
                    item.Selected = ob.Equals(property.Value);
                    itemList.Add(item);
                }

                itemList.Sort((a, b) => a.Text.CompareTo(b.Text));

                return formHelper.DoSelectHelper(property.Identifier, property.Label, false, itemList, property.Description);
            }
        }

        public override string RenderViewer()
        {
            EnumConverter enumConverter = property.EnumConverter;

            return (string)enumConverter.ConvertTo(null, CultureInfo.InvariantCulture, property.Value, typeof(string));
        }

    }
}

