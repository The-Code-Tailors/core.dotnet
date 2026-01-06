using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    public class FormHelper : Helper
    {
        public FormHelper(HtmlHelper htmlHelper) : base(htmlHelper) { }

        public MvcHtmlString DoCheckBox(string name, string text, bool isChecked)
        {
            return DoCheckBox(name, text, isChecked, null);
        }

        public MvcHtmlString DoCheckBox(string name, string text, bool isChecked, string popoverDescription)
        {
            return new MvcHtmlString(DoCheckBoxHelper(name, text, isChecked, popoverDescription));
        }

        public string DoCheckBoxHelper(string name, string text, bool isChecked, string popoverDescription)
        {
            return DoCheckBoxHelper(name, text, isChecked, popoverDescription, false);
        }

        public string DoCheckBoxHelper(string name, string text, bool isChecked, string popoverDescription, bool isReadOnly)
        {
            return DoCheckBoxHelper(name, text, isChecked, popoverDescription, isReadOnly, null);
        }

        public string DoCheckBoxHelper(string name, string text, bool isChecked, string popoverDescription, bool isReadOnly, string id, string value = "true")
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"checkbox\">\n");
            stringBuilder.Append("<label>");

            stringBuilder.Append("<input ");

            if (!string.IsNullOrWhiteSpace(id))
            {
                stringBuilder.Append("id=\"").Append(id).Append("\" ");
            }

            stringBuilder.Append("name=\"").Append(name).Append("\" ");

            stringBuilder.Append("type=\"checkbox\" ");

            stringBuilder.Append("value=\"").Append(value).Append("\" ");

            if (isChecked)
            {
                stringBuilder.Append("checked=\"checked\" ");
            }

            if (isReadOnly)
            {
                stringBuilder.Append("disabled=\"disabled\" ");
            }

            stringBuilder.Append("/>");

            //stringBuilder.Append("&nbsp;");
            stringBuilder.Append(text);
            stringBuilder.Append("</label>\n");
            AppendPopover(stringBuilder, text, popoverDescription);
            stringBuilder.Append("</div>\n");
            return stringBuilder.ToString();
        }

        public MvcHtmlString DoCheckboxList(string name, string text, Dictionary<string, string> dictionary, string[] values, string popoverDescription)
        {
            return new MvcHtmlString(DoCheckboxListHelper(name, text, dictionary, values, popoverDescription));
        }

        public string DoCheckboxListHelper(string name, string text, Dictionary<string, string> dictionary, string[] values, string popoverDescription)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"form-group\">\n");
            stringBuilder.Append("<label class=\"control-label\">").Append(text).Append("</label>\n");
            AppendPopover(stringBuilder, text, popoverDescription);
            stringBuilder.Append("<br />\n");
            int i = 0;
            foreach (string key in dictionary.Keys)
            {
                i++;
                bool isChecked = values != null && values.Contains(key);
                stringBuilder.Append(DoCheckBoxHelper(name, dictionary[key], isChecked, popoverDescription, false, $"{name}{i}", key));
            }
            stringBuilder.Append("</div>\n");
            return stringBuilder.ToString();
        }

        public MvcHtmlString DoDateTimePicker(string name, string text, DateTime? dateTme, string format, string popoverDescription)
        {
            return DoDateTimePicker(name, text, dateTme, format, popoverDescription, false);
        }

        /// <summary>
        /// Renders a "Bootstrap 3 Datepicker" (http://eonasdan.github.io/bootstrap-datetimepicker) field 
        /// </summary>
        /// <param name="name">The identifier and name of the field</param>
        /// <param name="text">The label of the field</param>
        /// <param name="dateTme">The value of the field - in order to leave the field empty, set this to null</param>
        /// <param name="format">Null for default (date and time), "L" for date only, "LT" for time only (also affects the glyphicon); see http://momentjs.com/docs/#/displaying/format/ for all valid formats</param>
        /// <param name="popoverDescription">The content of the popover; null for no popover</param>
        /// <param name="required">A Boolean value indicating if the field is required</param>
        /// <remarks>
        /// <para>The ID of the "Bootstrap 3 Datepicker" object will be "datetimepicker" followed by <paramref name="name" /></para>
        /// <para>Example</para>
        /// <code>
        /// $('#datetimepickerStart').on('dp.change', function (e) {
        ///     $('#datetimepickerEnd').data('DateTimePicker').minDate(e.date);
        /// });
        /// </code>
        /// </remarks>
        public MvcHtmlString DoDateTimePicker(string name, string text, DateTime? dateTme, string format, string popoverDescription, bool required)
        {
            return new MvcHtmlString(DoDateTimePickerHelper(name, text, dateTme, format, popoverDescription, required));
        }

        public static string DoDateTimePickerHelper(string name, string text, DateTime? dateTme, string format, string popoverDescription, bool required)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("class", "form-control");
            dictionary.Add("data-required", required.ToString().ToLower());

            string icon = "glyphicon glyphicon-calendar";
            string value = dateTme.HasValue ? dateTme.Value.ToString(Thread.CurrentThread.CurrentUICulture) : null;

            if (format == "LT")
            {
                icon = "glyphicon glyphicon-time";
                value = dateTme.HasValue ? dateTme.Value.ToString("HH:mm", Thread.CurrentThread.CurrentUICulture) : null;
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"form-group\">\n");
            stringBuilder.Append("    <label class=\"control-label\" for=\"").Append(name).Append("\">").Append(text).Append("</label>\n");
            AppendPopover(stringBuilder, text, popoverDescription);
            stringBuilder.Append("    <div class=\"input-group date\" id=\"datetimepicker").Append(name).Append("\">\n");
            stringBuilder.Append("        <span class=\"input-group-addon\"><span class=\"").Append(icon).Append("\"></span></span>\n");
            //stringBuilder.Append(InputExtensions.TextBox(htmlHelper, name, value, dictionary));
            stringBuilder.Append("<input class=\"form-control\" data-required=\"").Append(required.ToString().ToLower()).Append("\" id=\"").Append(name).Append("\" name=\"").Append(name).Append("\" type=\"text\" value=\"").Append(value).Append("\">");
            stringBuilder.Append("    </div>\n");
            stringBuilder.Append("</div>\n");
            stringBuilder.Append("<script type=\"text/javascript\">");
            stringBuilder.Append("    $(function () {");
            stringBuilder.Append("        $('#datetimepicker").Append(name).Append("').datetimepicker({");
            if (!string.IsNullOrWhiteSpace(format))
            {
                stringBuilder.Append("            format: '").Append(format).Append("',");
            }
            stringBuilder.Append("            locale: '").Append(Thread.CurrentThread.CurrentUICulture.ToString()).Append("',");
            stringBuilder.Append("        });");
            stringBuilder.Append("    });");
            stringBuilder.Append("</script>");
            return stringBuilder.ToString();
        }

        public string DoFileHelper(string name, string text, string popoverDescription)
        {
            return DoInputHelper(name, text, null, false, null, false, popoverDescription, "file");
        }

        protected string DoInputHelper(string name, string text, string placeholder, bool required, object value, bool @readonly, string popoverDescription, string type = "text")
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("class", "form-control");
            dictionary.Add("data-required", required.ToString().ToLower());

            if (!string.IsNullOrWhiteSpace(placeholder))
            {
                dictionary.Add("placeholder", placeholder);
            }
            if (@readonly)
            {
                dictionary.Add("disabled", null);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"form-group\">\n");
            stringBuilder.Append("    <label class=\"control-label\" for=\"").Append(name).Append("\">").Append(text).Append("</label>\n");
            AppendPopover(stringBuilder, text, popoverDescription);

            if (type == "file")
            {
                stringBuilder.Append($"<input name=\"{name}\" type=\"file\" />");  // class=\"form-control\" 
            }
            else
            {
                stringBuilder.Append(InputExtensions.TextBox(htmlHelper, name, value, dictionary));
            }

            stringBuilder.Append("</div>\n");
            return stringBuilder.ToString();
        }

        public MvcHtmlString DoPassword(string name, string text, bool required)
        {
            return DoPassword(name, text, null, required, null);
        }

        public MvcHtmlString DoPassword(string name, string text, string placeholder, bool required, string popoverDescription)
        {
            return new MvcHtmlString(DoPasswordHelper(name, text, placeholder, required, null, popoverDescription, false, null));
        }

        public MvcHtmlString DoPassword(string name, string text, string placeholder, bool required, string value, string popoverDescription, bool showToggleButton, string toggleText)
        {
            return new MvcHtmlString(DoPasswordHelper(name, text, placeholder, required, value, popoverDescription, showToggleButton, toggleText));
        }

        public string DoPasswordHelper(string name, string text, string placeholder, bool required, string value, string popoverDescription, bool showToggleButton, string toggleText)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("class", "form-control");
            dictionary.Add("data-required", required.ToString().ToLower());
            dictionary.Add("id", name);

            if (!string.IsNullOrWhiteSpace(placeholder))
            {
                dictionary.Add("placeholder", placeholder);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"form-group\">\n");
            stringBuilder.Append("    <label class=\"control-label\" for=\"").Append(name).Append("\">").Append(text).Append("</label>\n");
            AppendPopover(stringBuilder, text, popoverDescription);
            if (showToggleButton)
            {
                stringBuilder.Append("    <div class=\"input-group\">\n");
                stringBuilder.Append(InputExtensions.Password(htmlHelper, name, value, dictionary));
                stringBuilder.Append("        <div class=\"input-group-btn\">\n");
                stringBuilder.Append("            <button class=\"btn btn-default\" id=\"fs-core-toggle-password-visibility-").Append(name).Append("\" title=\"").Append(toggleText).Append("\" type=\"button\"><span class=\"glyphicon glyphicon-eye-open\"></span></button>");
                stringBuilder.Append("        </div>\n");
                stringBuilder.Append("    </div>\n");
                stringBuilder.Append("    <script>\n");
                stringBuilder.Append("        $(function () {\n");
                stringBuilder.Append("            $('#fs-core-toggle-password-visibility-").Append(name).Append("').on('click', function () {\n");
                stringBuilder.Append("                var item = $('#").Append(name).Append("');\n");
                stringBuilder.Append("                switch (item.attr('type')) {\n");
                stringBuilder.Append("                    case 'text':\n");
                stringBuilder.Append("                        item.attr('type', 'password');\n");
                stringBuilder.Append("                        $('.glyphicon-eye-close', this).removeClass('glyphicon-eye-close').addClass('glyphicon-eye-open');\n");
                stringBuilder.Append("                        break;\n");
                stringBuilder.Append("                    case 'password':\n");
                stringBuilder.Append("                        item.attr('type', 'text');\n");
                stringBuilder.Append("                        $('.glyphicon-eye-open', this).removeClass('glyphicon-eye-open').addClass('glyphicon-eye-close');\n");
                stringBuilder.Append("                    break;\n");
                stringBuilder.Append("                }\n");
                stringBuilder.Append("            });\n");
                stringBuilder.Append("        });\n");
                stringBuilder.Append("    </script>\n");
            }
            else
            {
                stringBuilder.Append(InputExtensions.Password(htmlHelper, name, "", dictionary));
            }
            stringBuilder.Append("</div>\n");
            return stringBuilder.ToString();
        }

        public MvcHtmlString DoRadioList(string name, string text, Dictionary<string, string> dictionary, string value, string popoverDescription)
        {
            return new MvcHtmlString(DoRadioListHelper(name, text, dictionary, value, popoverDescription));
        }

        public string DoRadioListHelper(string name, string text, Dictionary<string, string> dictionary, string value, string popoverDescription)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"form-group\">\n");
            stringBuilder.Append("<label class=\"control-label\">").Append(text).Append("</label>\n");
            AppendPopover(stringBuilder, text, popoverDescription);
            //stringBuilder.Append("<br />\n");
            int i = 0;
            foreach (string key in dictionary.Keys)
            {
                i++;
                //stringBuilder.Append("<label class=\"radio-inline\">");
                //stringBuilder.Append("<input");
                //stringBuilder.Append(" id=\"").AppendFormat("{0}{1}", name, i).Append("\"");
                //stringBuilder.Append(" name=\"").Append(name).Append("\"");
                //stringBuilder.Append(" type=\"radio\"");
                //stringBuilder.Append(" value=\"").Append(key).Append("\"");
                //stringBuilder.Append(" ").Append(key == value ? " checked" : "").Append("");
                //stringBuilder.Append("/>");
                //stringBuilder.Append("&nbsp;");
                //stringBuilder.Append(dictionary[key]);
                //stringBuilder.Append("</label>\n");
                stringBuilder.Append("<div class=\"radio\">\n");
                stringBuilder.Append("<label>\n");
                stringBuilder.Append("<input");
                stringBuilder.Append(" id=\"").AppendFormat("{0}{1}", name, i).Append("\"");
                stringBuilder.Append(" name=\"").Append(name).Append("\"");
                stringBuilder.Append(" type=\"radio\"");
                stringBuilder.Append(" value=\"").Append(key).Append("\"");
                stringBuilder.Append(" ").Append(key == value ? " checked" : "").Append("");
                stringBuilder.Append("/>");
                stringBuilder.Append("&nbsp;");
                stringBuilder.Append(dictionary[key]);
                stringBuilder.Append("</label>\n");
                stringBuilder.Append("</div>\n");
            }
            stringBuilder.Append("</div>\n");
            return stringBuilder.ToString();
        }

        public MvcHtmlString DoSelect(string name, string text, bool required, IEnumerable<SelectListItem> selectList)
        {
            return DoSelect(name, text, required, selectList, null);
        }

        public MvcHtmlString DoSelect(string name, string text, bool required, IEnumerable<SelectListItem> selectList, string popoverDescription)
        {
            return new MvcHtmlString(DoSelectHelper(name, text, required, selectList, popoverDescription));
        }

        public string DoSelectHelper(string name, string text, bool required, IEnumerable<SelectListItem> selectList, string popoverDescription)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("class", "form-control");
            dictionary.Add("data-required", required.ToString().ToLower());

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"form-group\">\n");
            stringBuilder.Append("    <label class=\"control-label\" for=\"").Append(name).Append("\">").Append(text).Append("</label>\n");
            AppendPopover(stringBuilder, text, popoverDescription);
            stringBuilder.Append(SelectExtensions.DropDownList(htmlHelper, name, selectList, dictionary));
            stringBuilder.Append("</div>\n");
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Renders a text-input field 
        /// </summary>
        /// <param name="name">The identifier and name of the field</param>
        /// <param name="text">The label of the field</param>
        /// <param name="required">A Boolean value indicating if the field is required</param>
        /// <param name="value">The value of the field - in order to leave the field empty, set this to null</param>
        public MvcHtmlString DoText(string name, string text, bool required, object value)
        {
            return DoText(name, text, required, value, false);
        }

        /// <summary>
        /// Renders a text-input field with a placeholder 
        /// </summary>
        /// <param name="name">The identifier and name of the field</param>
        /// <param name="text">The label of the field</param>
        /// <param name="placeholder">The placeholder of the field</param>
        /// <param name="required">A Boolean value indicating if the field is required</param>
        /// <param name="value">The value of the field - in order to leave the field empty, set this to null</param>
        public MvcHtmlString DoText(string name, string text, string placeholder, bool required, object value)
        {
            return DoText(name, text, placeholder, required, value, false);
        }

        /// <summary>
        /// Renders a(n optionally) read-only text-input field 
        /// </summary>
        /// <param name="name">The identifier and name of the field</param>
        /// <param name="text">The label of the field</param>
        /// <param name="required">A Boolean value indicating if the field is required</param>
        /// <param name="value">The value of the field - this is not expected to be null</param>
        /// <param name="readonly">A Boolean value indicating if the field is read-only</param>
        public MvcHtmlString DoText(string name, string text, bool required, object value, bool @readonly)
        {
            return DoText(name, text, null, required, value, @readonly);
        }

        protected MvcHtmlString DoText(string name, string text, string placeholder, bool required, object value, bool @readonly)
        {
            return DoText(name, text, placeholder, required, value, @readonly, null);
        }

        public MvcHtmlString DoText(string name, string text, string placeholder, bool required, object value, bool @readonly, string popoverDescription)
        {
            return new MvcHtmlString(DoTextHelper(name, text, placeholder, required, value, @readonly, popoverDescription));
        }

        public string DoTextHelper(string name, string text, string placeholder, bool required, object value, bool @readonly, string popoverDescription)
        {
            return DoInputHelper(name, text, placeholder, required, value, @readonly, popoverDescription);
        }

        public MvcHtmlString DoTextArea(string name, string text, bool required, string value)
        {
            return DoTextArea(name, text, null, required, value, false);
        }

        public MvcHtmlString DoTextArea(string name, string text, string placeholder, bool required, string value)
        {
            return DoTextArea(name, text, placeholder, required, value, false);
        }

        public MvcHtmlString DoTextArea(string name, string text, string placeholder, bool required, string value, bool @readonly)
        {
            return DoTextArea(name, text, placeholder, required, value, false, null);
        }

        public MvcHtmlString DoTextArea(string name, string text, string placeholder, bool required, string value, bool @readonly, string popoverDescription)
        {
            return new MvcHtmlString(DoTextAreaHelper(name, text, placeholder, required, value, @readonly, popoverDescription));
        }

        public string DoTextAreaHelper(string name, string text, string placeholder, bool required, string value, bool @readonly, string popoverDescription)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("class", "form-control");
            dictionary.Add("data-required", required.ToString().ToLower());

            if (!string.IsNullOrWhiteSpace(placeholder))
            {
                dictionary.Add("placeholder", placeholder);
            }
            if (@readonly)
            {
                dictionary.Add("readonly", null);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"form-group\">\n");
            stringBuilder.Append("    <label class=\"control-label\" for=\"").Append(name).Append("\">").Append(text).Append("</label>\n");
            AppendPopover(stringBuilder, text, popoverDescription);
            MvcHtmlString temp = TextAreaExtensions.TextArea(htmlHelper, name, value, dictionary);
            stringBuilder.Append(temp);
            stringBuilder.Append("</div>\n");
            return stringBuilder.ToString();
        }

        public static void AppendPopover(StringBuilder stringBuilder, string title, string description)
        {
            // "For proper cross-browser and cross-platform behavior, you must use the <a> tag, not the <button> tag, 
            // and you also must include the role="button" and tabindex attributes"

            if (!string.IsNullOrWhiteSpace(description))
            {
                stringBuilder.Append("<a data-content=\"").Append(WebUtility.HtmlEncode(description)).Append("\" data-toggle=\"popover\" data-trigger=\"focus\" title=\"").Append(WebUtility.HtmlEncode(title)).Append("\" role=\"button\" tabindex=\"0\"><span class=\"glyphicon glyphicon-info-sign\"></span></a>\n");
            }
        }

    }
}

