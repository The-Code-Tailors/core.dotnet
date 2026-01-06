using System;
using System.Threading;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers.PropertyGridHelperPropertyRenderers
{
    public class DateTimePropertyRenderer : PropertyRenderer
    {
        public DateTimePropertyRenderer(PropertyGridHelperProperty property) : base(property) { }

        public override object ParseValue(string s)
        {
            object result = null;

            if (DateTime.TryParse(s, out DateTime dateTimeResult))
            {
                result = new DateTime?(dateTimeResult);
            }

            return result;
        }

        public override string RenderEditor(FormHelper formHelper)
        {
            if (property.IsReadOnly)
            {
                string value = null;

                DateTime? dateTime = GetValue();

                if (dateTime.HasValue)
                {
                    value = dateTime.Value.ToString(DoFormat(), Thread.CurrentThread.CurrentUICulture);
                }

                return formHelper.DoTextHelper(property.Identifier, property.Label, null, false, value, true, property.Description);
            }
            else
            {
                DateTime? dateTime = GetValue();

                string format = null;  // Null for default (date and time) 

                switch (property.EditorType)
                {
                    case PropertyGridHelperEditorType.DatePicker:
                        format = "L";
                        break;
                    case PropertyGridHelperEditorType.TimePicker:
                        format = "LT";
                        break;
                }

                return FormHelper.DoDateTimePickerHelper(property.Identifier, property.Label, dateTime, format, property.Description, false);
            }
        }

        public override string RenderViewer()
        {
            string result = null;

            DateTime? dateTime = GetValue();

            if (dateTime.HasValue)
            {
                result = dateTime.Value.ToString(DoFormat(), Thread.CurrentThread.CurrentUICulture);
            }

            return result;
        }

        protected string DoFormat()
        {
            string format = "g";

            switch (property.EditorType)
            {
                case PropertyGridHelperEditorType.DatePicker:
                    format = "d";
                    break;
                case PropertyGridHelperEditorType.TimePicker:
                    format = "t";
                    break;
            }

            return format;
        }

        protected DateTime? GetValue()
        {
            DateTime? dateTime = null;

            if (property.Value != null && (DateTime)property.Value != default(DateTime))
            {
                dateTime = new DateTime?((DateTime)property.Value);
            }

            return dateTime;
        }

    }
}

