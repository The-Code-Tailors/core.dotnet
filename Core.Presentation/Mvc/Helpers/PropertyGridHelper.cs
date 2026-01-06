using com.fabioscagliola.Core.Data;
using com.fabioscagliola.Core.DataAccess;
using com.fabioscagliola.Core.Presentation.Mvc.Helpers.PropertyGridHelperPropertyRenderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    /// <summary>
    /// Exposes methods to render a property-grid editor and a property-grid viewer, 
    /// as well as static methods to update the properties of an object edited using a property-grid editor 
    /// </summary>
    public class PropertyGridHelper : Helper
    {
        public PropertyGridHelper(HtmlHelper htmlHelper) : base(htmlHelper) { }

        /// <summary>
        /// Renders a property-grid editor including all the properties 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="customizePropertyLabel">An optional custom method to be invoked in order to render properties' labels</param>
        /// <param name="customizePropertyValue">An optional custom method to be invoked in order to render properties' values</param>
        public MvcHtmlString DoPropertyGridEditor(PropertyGridHelperData data, Milieu milieu = null, Func<PropertyGridHelperProperty, string> customizePropertyLabel = null, Func<PropertyGridHelperProperty, string> customizePropertyValue = null)
        {
            return new MvcHtmlString(DoPropertyGridEditorHelper(data, null, milieu, customizePropertyLabel, customizePropertyValue));
        }

        /// <summary>
        /// Renders a property-grid editor including the specified properties 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="propertyList">The properties to be included in the editor</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="customizePropertyLabel">An optional custom method to be invoked in order to render properties' labels</param>
        /// <param name="customizePropertyValue">An optional custom method to be invoked in order to render properties' values</param>
        public MvcHtmlString DoPropertyGridEditor(PropertyGridHelperData data, List<PropertyGridHelperProperty> propertyList, Milieu milieu = null, Func<PropertyGridHelperProperty, string> customizePropertyLabel = null, Func<PropertyGridHelperProperty, string> customizePropertyValue = null)
        {
            return new MvcHtmlString(DoPropertyGridEditorHelper(data, propertyList, milieu, customizePropertyLabel, customizePropertyValue));
        }

        /// <summary>
        /// Renders a property-grid editor including all the properties 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="customizePropertyLabel">An optional custom method to be invoked in order to render properties' labels</param>
        /// <param name="customizePropertyValue">An optional custom method to be invoked in order to render properties' values</param>
        public string DoPropertyGridEditorHelper(PropertyGridHelperData data, Milieu milieu = null, Func<PropertyGridHelperProperty, string> customizePropertyLabel = null, Func<PropertyGridHelperProperty, string> customizePropertyValue = null)
        {
            return DoPropertyGridEditorHelper(data, null, milieu, customizePropertyLabel, customizePropertyValue);
        }

        /// <summary>
        /// Renders a property-grid editor including the specified properties 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="propertyList">The properties to be included in the editor</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="customizePropertyLabel">An optional method to be invoked when rendering each property's label</param>
        /// <param name="customizePropertyValue">An optional method to be invoked when rendering each property's value</param>
        public string DoPropertyGridEditorHelper(PropertyGridHelperData data, List<PropertyGridHelperProperty> propertyList, Milieu milieu = null, Func<PropertyGridHelperProperty, string> customizePropertyLabel = null, Func<PropertyGridHelperProperty, string> customizePropertyValue = null)
        {
            return Render(data, null, milieu ?? Milieu.SystemMilieu, RenderPropertyGridEditor, customizePropertyLabel, customizePropertyValue);
        }

        /// <summary>
        /// Renders a property-grid viewer including all the properties 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="customizePropertyLabel">An optional custom method to be invoked in order to render properties' labels</param>
        /// <param name="customizePropertyValue">An optional custom method to be invoked in order to render properties' values</param>
        public MvcHtmlString DoPropertyGridViewer(PropertyGridHelperData data, Milieu milieu = null, Func<PropertyGridHelperProperty, string> customizePropertyLabel = null, Func<PropertyGridHelperProperty, string> customizePropertyValue = null)
        {
            return new MvcHtmlString(DoPropertyGridViewerHelper(data, null, milieu, customizePropertyLabel, customizePropertyValue));
        }

        /// <summary>
        /// Renders a property-grid viewer including the specified properties 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="propertyList">The properties to be included in the viewer</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="customizePropertyLabel">An optional custom method to be invoked in order to render properties' labels</param>
        /// <param name="customizePropertyValue">An optional custom method to be invoked in order to render properties' values</param>
        public MvcHtmlString DoPropertyGridViewer(PropertyGridHelperData data, List<PropertyGridHelperProperty> propertyList, Milieu milieu = null, Func<PropertyGridHelperProperty, string> customizePropertyLabel = null, Func<PropertyGridHelperProperty, string> customizePropertyValue = null)
        {
            return new MvcHtmlString(DoPropertyGridViewerHelper(data, propertyList, milieu, customizePropertyLabel, customizePropertyValue));
        }

        /// <summary>
        /// Renders a property-grid viewer including all the properties 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="customizePropertyLabel">An optional custom method to be invoked in order to render properties' labels</param>
        /// <param name="customizePropertyValue">An optional custom method to be invoked in order to render properties' values</param>
        public string DoPropertyGridViewerHelper(PropertyGridHelperData data, Milieu milieu = null, Func<PropertyGridHelperProperty, string> customizePropertyLabel = null, Func<PropertyGridHelperProperty, string> customizePropertyValue = null)
        {
            return DoPropertyGridViewerHelper(data, null, milieu, customizePropertyLabel, customizePropertyValue);
        }

        /// <summary>
        /// Renders a property-grid viewer including the specified properties 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="propertyList">The properties to be included in the viewer</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="customizePropertyLabel">An optional method to be invoked when rendering each property's label</param>
        /// <param name="customizePropertyValue">An optional method to be invoked when rendering each property's value</param>
        public string DoPropertyGridViewerHelper(PropertyGridHelperData data, List<PropertyGridHelperProperty> propertyList, Milieu milieu = null, Func<PropertyGridHelperProperty, string> customizePropertyLabel = null, Func<PropertyGridHelperProperty, string> customizePropertyValue = null)
        {
            return Render(data, propertyList, milieu ?? Milieu.SystemMilieu, RenderPropertyGridViewer, customizePropertyLabel, customizePropertyValue);
        }

        /// <summary>
        /// Renders a property-grid editor or a property-grid viewer using the <paramref name="renderContents"/> method to render the contents of the editor or viewer (for each category), including the specified properties 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="propertyList">The properties to be included in the editor or viewer</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="renderContents">The method used to render the contents of the editor or viewer (for each category)</param>
        /// <param name="customizePropertyLabel">An optional method to be invoked when rendering each property's label</param>
        /// <param name="customizePropertyValue">An optional method to be invoked when rendering each property's value</param>
        protected string Render(PropertyGridHelperData data, List<PropertyGridHelperProperty> propertyList, Milieu milieu, Action<StringBuilder, List<PropertyGridHelperProperty>, Milieu, int, Func<PropertyGridHelperProperty, string>, Func<PropertyGridHelperProperty, string>> renderContents, Func<PropertyGridHelperProperty, string> customizePropertyLabel = null, Func<PropertyGridHelperProperty, string> customizePropertyValue = null)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (propertyList == null)
            {
                propertyList = PropertyGridHelperProperty.DoPropertyList(data.Object);
            }

            if (propertyList.GroupBy(x => x.Category).Count() == 1)
            {
                renderContents(stringBuilder, propertyList, milieu, data.Cols, customizePropertyLabel, customizePropertyValue);
            }
            else
            {
                //stringBuilder.Append("<div class=\"panel-group\" id=\"propertyGrid\">");

                List<IGrouping<string, PropertyGridHelperProperty>> grList = propertyList.GroupBy(x => x.Category, x => x).ToList();

                grList.Sort((a, b) =>
                {
                    int result = 0;

                    if (a.Key != null)
                    {
                        result = PropertyGridHelperProperty.DoVersionComparison(a.Key, b.Key);
                        if (result == 0)
                        {
                            result = a.Key.CompareTo(b.Key);
                        }
                    }

                    return result;
                });

                foreach (var gr in grList)
                {
                    AppendPanel(stringBuilder, gr.Key, () =>
                    {
                        renderContents(stringBuilder, gr.ToList(), milieu, data.Cols, customizePropertyLabel, customizePropertyValue);
                    });
                }

                //stringBuilder.Append("</div>");  // panel-group 
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Render the contents of the editor 
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="propertyList">The properties to be included in the editor or viewer</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="cols">The number of columns in which the properties are to be arranged in the editor</param>
        /// <param name="customizePropertyLabel">An optional method to be invoked when rendering each property's label</param>
        /// <param name="customizePropertyValue">An optional method to be invoked when rendering each property's value</param>
        protected void RenderPropertyGridEditor(StringBuilder stringBuilder, List<PropertyGridHelperProperty> propertyList, Milieu milieu, int cols, Func<PropertyGridHelperProperty, string> customizePropertyLabel, Func<PropertyGridHelperProperty, string> customizePropertyValue)
        {
            stringBuilder.Append("<div class=\"row\">");

            foreach (PropertyGridHelperProperty property in propertyList)
            {
                stringBuilder.Append($"<div class=\"col-sm-{ 12 / cols }\">");

                if (property.PropertyList.Count == 0)
                {
                    if (customizePropertyValue == null)
                    {
                        stringBuilder.Append(RenderPropertyGridEditorProperty(property, milieu));
                    }
                    else
                    {
                        stringBuilder.Append(customizePropertyValue(property));
                    }
                }
                else
                {
                    string propertyLabel = property.Label;

                    if (customizePropertyLabel != null)
                    {
                        propertyLabel = customizePropertyLabel(property);
                    }

                    AppendPanel(stringBuilder, propertyLabel, () =>
                    {
                        RenderPropertyGridEditor(stringBuilder, property.PropertyList, milieu, cols, customizePropertyLabel, customizePropertyValue);
                    });
                }

                stringBuilder.Append("</div>");  // col 
            }

            stringBuilder.Append("</div>");  // row 
        }

        /// <summary>
        /// Render the contents of the viewer 
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="propertyList">The properties to be included in the editor or viewer</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        /// <param name="cols">NOT IN USE</param>
        /// <param name="customizePropertyLabel">An optional method to be invoked when rendering each property's label</param>
        /// <param name="customizePropertyValue">An optional method to be invoked when rendering each property's value</param>
        protected void RenderPropertyGridViewer(StringBuilder stringBuilder, List<PropertyGridHelperProperty> propertyList, Milieu milieu, int cols, Func<PropertyGridHelperProperty, string> customizePropertyLabel, Func<PropertyGridHelperProperty, string> customizePropertyValue)
        {
            stringBuilder.Append("<table class=\"table table-condensed table-hover table-striped propertygrid\">");
            stringBuilder.Append("<colgroup>");
            stringBuilder.Append("<col />");
            stringBuilder.Append("<col />");
            stringBuilder.Append("</colgroup>");
            stringBuilder.Append("<tbody>");

            foreach (PropertyGridHelperProperty property in propertyList)
            {
                string propertyLabel = property.Label;

                if (customizePropertyLabel != null)
                {
                    propertyLabel = customizePropertyLabel(property);
                }

                string propertyValue = property.FormattedValue?
                    .Replace("\r\n", "<br />")
                    .Replace("\n", "<br />");

                if (Type.GetTypeCode(property.Type) == TypeCode.String && property.EditorType == PropertyGridHelperEditorType.Password)
                {
                    propertyValue = "&bull;&bull;&bull;&bull;&bull;&bull;&bull;&bull;";
                }
                else if (customizePropertyValue != null)
                {
                    propertyValue = customizePropertyValue(property);
                }
                else
                {
                    PropertyRenderer renderer = PropertyRenderer.CreateInstance(property, milieu);

                    if (renderer != null)
                    {
                        propertyValue = renderer.RenderViewer();
                    }
                }

                stringBuilder.Append("<tr>");
                stringBuilder.Append("<th class=\"text-nowrap\">");
                stringBuilder.Append(propertyLabel);
                stringBuilder.Append("&nbsp;");
                FormHelper.AppendPopover(stringBuilder, propertyLabel, property.Description);
                stringBuilder.Append("</th>");
                stringBuilder.Append("<td>");

                if (property.PropertyList.Count == 0)
                {
                    stringBuilder.Append(propertyValue);
                }
                else
                {
                    RenderPropertyGridViewer(stringBuilder, property.PropertyList, milieu, cols, customizePropertyLabel, customizePropertyValue);
                }

                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }

            stringBuilder.Append("</tbody>");
            stringBuilder.Append("</table>");
        }

        /// <summary>
        /// Renders the editor of an individual property 
        /// </summary>
        /// <param name="property">The property whose editor is to be rendered</param>
        /// <param name="milieu">The milieu used to retrieve the parent entity of foreign-key properties</param>
        protected string RenderPropertyGridEditorProperty(PropertyGridHelperProperty property, Milieu milieu)
        {
            string result = null;

            FormHelper formHelper = new FormHelper(htmlHelper);

            PropertyRenderer renderer = PropertyRenderer.CreateInstance(property, milieu);

            if (renderer != null)
            {
                result = renderer.RenderEditor(formHelper);
            }
            else
            {
                switch (Type.GetTypeCode(property.Type))
                {
                    case TypeCode.Byte:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                    case TypeCode.Single:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                    case TypeCode.String:
                        if (Type.GetTypeCode(property.Type) == TypeCode.String)
                        {
                            switch (property.EditorType)
                            {
                                case PropertyGridHelperEditorType.Password:
                                    result = formHelper.DoPasswordHelper(property.Identifier, property.Label, null, false, property.FormattedValue, property.Description, true, null);
                                    break;
                                case PropertyGridHelperEditorType.TextArea:
                                    result = formHelper.DoTextAreaHelper(property.Identifier, property.Label, null, false, (string)property.Value, property.IsReadOnly, property.Description);
                                    break;
                                default:
                                    result = formHelper.DoTextHelper(property.Identifier, property.Label, null, false, property.FormattedValue, property.IsReadOnly, property.Description);  // Duplicate line {402311C9-E61A-4D57-959D-509B2734C41C} 
                                    break;
                            }
                        }
                        else
                        {
                            result = formHelper.DoTextHelper(property.Identifier, property.Label, null, false, property.FormattedValue, property.IsReadOnly, property.Description);  // Duplicate line {402311C9-E61A-4D57-959D-509B2734C41C} 
                        }
                        break;

                    case TypeCode.Boolean:
                        result = formHelper.DoCheckBoxHelper(property.Identifier, property.Label, (bool)property.Value, property.Description, property.IsReadOnly);
                        break;

                    case TypeCode.DateTime:
                        // See above 
                        break;

                    case TypeCode.Char:
                    case TypeCode.DBNull:
                    case TypeCode.Empty:
                    case TypeCode.Object:
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Updates all the properties of an object edited using a property-grid editor 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="request">The HTTP request from the client</param>
        public static void UpdateObject(PropertyGridHelperData data, HttpRequestBase request)
        {
            List<PropertyGridHelperProperty> propertyList = PropertyGridHelperProperty.DoPropertyList(data.Object);

            List<PropertyGridHelperPropertyExInfo> propertiesExceptions = new List<PropertyGridHelperPropertyExInfo>();

            UpdateObject(data, request, propertyList, propertiesExceptions);

            if (propertiesExceptions.Count != 0)
            {
                throw new PropertyGridHelperException(propertiesExceptions);
            }
        }

        /// <summary>
        /// Updates the specified properties of an object edited using a property-grid editor 
        /// </summary>
        /// <param name="data">The configuration of the property-grid editor or viewer</param>
        /// <param name="request">The HTTP request from the client</param>
        /// <param name="propertyList">The properties to be updated</param>
        /// <param name="propertiesExceptions">A dictionary to collect any exception thrown while updating the properties of the object edited using the property-grid editor</param>
        protected static void UpdateObject(PropertyGridHelperData data, HttpRequestBase request, List<PropertyGridHelperProperty> propertyList, List<PropertyGridHelperPropertyExInfo> propertiesExceptions)
        {
            foreach (PropertyGridHelperProperty property in propertyList)
            {
                if (!property.IsReadOnly)
                {
                    if (request.Params.AllKeys.Contains(property.Identifier))
                    {
                        string stringValue = request.Params[property.Identifier];

                        try
                        {
                            object value;

                            PropertyRenderer renderer = PropertyRenderer.CreateInstance(property);

                            if (renderer != null)
                            {
                                value = renderer.ParseValue(stringValue);
                            }
                            else
                            {
                                value = Convert.ChangeType(stringValue, property.Type);
                            }

                            property.PropertyInfo.SetValue(data.Object, value);

                            if (property.EditorType == PropertyGridHelperEditorType.CustomLookup && property.DisplayPropertyInfo != null && property.DisplayPropertyInfo.GetSetMethod() != null)
                            {
                                string stringDisplayValue = request.Params[((CustomLookupPropertyRenderer)renderer).DisplayPropertyIdentifier];
                                object displayValue = Convert.ChangeType(stringDisplayValue, property.DisplayPropertyInfo.PropertyType);
                                property.DisplayPropertyInfo.SetValue(data.Object, displayValue);
                            }
                        }
                        catch (Exception e)
                        {
                            propertiesExceptions.Add(new PropertyGridHelperPropertyExInfo(property, e.InnerException ?? e));
                        }
                    }
                    else
                    {
                        if (property.Type == typeof(bool))
                        {
                            property.PropertyInfo.SetValue(data.Object, false);
                        }
                    }
                }

                if (property.Type.Module.Name != "mscorlib.dll" && property.PropertyList.Count != 0)
                {
                    UpdateObject(new PropertyGridHelperData() { Object = property.Value, }, request, property.PropertyList, propertiesExceptions);
                }
            }

            if (data.Object is IDataErrorInfo item)
            {
                List<ValidationError> validationErrors = ValidateableObjectHelper.GetValidationErrors(item);

                // Iterate on the properties rather than on the errors in order to honor the properties' sequence numbers in the list of the errors 

                if (validationErrors.Count != 0)
                {
                    foreach (PropertyGridHelperProperty property in propertyList)
                    {
                        ValidationError validationError = validationErrors.Find(x => x.PropertyInfo.Name == property.Name);

                        if (validationError != null)
                        {
                            PresentationException e = new PresentationException(validationError.ErrorMessage);
                            propertiesExceptions.Add(new PropertyGridHelperPropertyExInfo(property, e));
                        }
                    }
                }
            }
        }

        protected void AppendPanel(StringBuilder stringBuilder, string title, Action renderBody)
        {
            Guid guid = Guid.NewGuid();

            stringBuilder.Append("<div class=\"panel panel-default\">");
            stringBuilder.Append("<div class=\"panel-heading\">");
            stringBuilder.Append("<h3 class=\"panel-title\">");
            stringBuilder.Append($"<a data-toggle=\"collapse\" href=\"#{ guid.ToByteArray().ToHexString() }\">");  // data-parent=\"#propertyGrid\" 
            stringBuilder.Append(title);
            stringBuilder.Append("</a>");
            stringBuilder.Append("</h3>");
            stringBuilder.Append("</div>");  // panel-heading 
            stringBuilder.Append($"<div id=\"{ guid.ToByteArray().ToHexString() }\" class=\"panel-collapse collapse in\">");
            stringBuilder.Append("<div class=\"panel-body\">");

            renderBody();

            stringBuilder.Append("</div>");  // panel-body 
            stringBuilder.Append("</div>");  // panel-collapse collapse 
            stringBuilder.Append("</div>");  // panel panel-default 
        }

    }
}

