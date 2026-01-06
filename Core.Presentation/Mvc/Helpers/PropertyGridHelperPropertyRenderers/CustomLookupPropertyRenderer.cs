using System;
using System.Collections.Generic;
using System.Text;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers.PropertyGridHelperPropertyRenderers
{
    public class CustomLookupPropertyRenderer : PropertyRenderer
    {
        protected const string SEPARATOR = ",";

        public CustomLookupPropertyRenderer(PropertyGridHelperProperty property) : base(property) { }

        public string DisplayPropertyIdentifier
        {
            get
            {
                return $"{property.Identifier}__CustomLookup";
            }
        }

        public override object ParseValue(string s)
        {
            // Single selection 
            if (property.Type == typeof(long?))
            {
                if (long.TryParse(s, out long result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }

            // Multiple selection: list of longs 
            else if (property.Type.IsGenericType && Type.GetTypeCode(property.Type.GenericTypeArguments[0]) == TypeCode.Int64)
            {
                List<long> result = new List<long>();

                string[] values = s.Split(SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string value in values)
                    if (long.TryParse(value, out long temp))
                        result.Add(temp);

                return result;
            }

            // Multiple selection: list of enums 
            else if (property.Type.IsGenericType && property.Type.GenericTypeArguments[0].IsEnum)
            {
                object result = Activator.CreateInstance(property.Type);

                string[] values = s.Split(SEPARATOR.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string value in values)
                    if (int.TryParse(value, out int temp))
                        result.GetType().GetMethod("Add").Invoke(result, new[] { property.Type.GenericTypeArguments[0].GetFields()[temp].GetValue(null) });

                return result;
            }

            return base.ParseValue(s);
        }

        public override string RenderEditor(FormHelper formHelper)
        {
            string hiddenFieldValue = null;
            string fieldValue = null;

            if (property.Value is System.Collections.IEnumerable enumerable)
            {
                // Multiple selection: list of longs 
                if (property.Type.IsGenericType && Type.GetTypeCode(property.Type.GenericTypeArguments[0]) == TypeCode.Int64)
                {
                    List<long> list = new List<long>();

                    foreach (long item in enumerable)
                        list.Add(item);

                    hiddenFieldValue = string.Join(SEPARATOR, list);
                }

                // Multiple selection: list of enums 
                else if (property.Type.IsGenericType && property.Type.GenericTypeArguments[0].IsEnum)
                {
                    List<int> list = new List<int>();

                    foreach (int item in enumerable)
                        list.Add(item);

                    hiddenFieldValue = string.Join(SEPARATOR, list);
                }
            }

            if (hiddenFieldValue == null && property.Value != null) hiddenFieldValue = property.Value.ToString();

            if (property.DisplayValue != null) fieldValue = property.DisplayValue.ToString();

            if (property.Type == typeof(long) && (long)property.Value == 0) fieldValue = "";

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"<input id=\"{property.Identifier}\" name=\"{property.Identifier}\" type=\"hidden\" value=\"{hiddenFieldValue}\" />\r\n");
            stringBuilder.Append($"<div class=\"form-group fs-core-lookup\">\r\n");
            stringBuilder.Append($"    <label class=\"control-label\" for=\"{DisplayPropertyIdentifier}\">{property.Label}</label>\r\n");
            if (!string.IsNullOrWhiteSpace(property.Description))
                stringBuilder.Append($"    <a data-content=\"{property.Description}\" data-toggle=\"popover\" data-trigger=\"focus\" title=\"{property.Label}\" role=\"button\" tabindex=\"0\"><span class=\"glyphicon glyphicon-info-sign\"></span></a>\r\n");
            stringBuilder.Append($"    <div class=\"input-group\">\r\n");
            stringBuilder.Append($"        <span class=\"input-group-addon\" id=\"{DisplayPropertyIdentifier}__Button\" title=\"\">\r\n");
            stringBuilder.Append($"            <span class=\"glyphicon glyphicon-option-horizontal\"></span>\r\n");
            stringBuilder.Append($"        </span>\r\n");
            stringBuilder.Append($"        <input class=\"form-control\" readonly=\"readonly\" id=\"{DisplayPropertyIdentifier}\" name=\"{DisplayPropertyIdentifier}\" value=\"{fieldValue}\" />\r\n");
            stringBuilder.Append($"    </div>\r\n");
            stringBuilder.Append($"</div>\r\n");

            return stringBuilder.ToString();
        }

        public override string RenderViewer()
        {
            string result = null;

            if (property.Value != null)
            {
                result = property.Value.ToString();

                if (property.DisplayValue != null)
                {
                    result = property.DisplayValue.ToString();
                }
            }

            return result;
        }

    }
}

