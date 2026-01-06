using com.fabioscagliola.Core.DataAccess;
using System;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers.PropertyGridHelperPropertyRenderers
{
    public abstract class PropertyRenderer
    {
        protected PropertyGridHelperProperty property;

        protected PropertyRenderer(PropertyGridHelperProperty property)
        {
            this.property = property;
        }

        public virtual object ParseValue(string s)
        {
            return Convert.ChangeType(s, property.Type);
        }

        public abstract string RenderEditor(FormHelper formHelper);

        public abstract string RenderViewer();

        public static PropertyRenderer CreateInstance(PropertyGridHelperProperty property)
        {
            return CreateInstance(property, Milieu.SystemMilieu);
        }

        public static PropertyRenderer CreateInstance(PropertyGridHelperProperty property, Milieu milieu)
        {
            PropertyRenderer renderer = null;

            if (property.ParentType != null)
            {
                renderer = new LookupPropertyRenderer(property, milieu);
            }
            else if (property.EditorType == PropertyGridHelperEditorType.Illustration)
            {
                renderer = new IllustrationPropertyRenderer(property);
            }
            else if (property.Type.IsEnum)
            {
                renderer = new EnumPropertyRenderer(property);
            }
            else if (Type.GetTypeCode(property.Type) == TypeCode.DateTime || (property.Type.IsGenericType && Type.GetTypeCode(property.Type.GenericTypeArguments[0]) == TypeCode.DateTime))
            {
                renderer = new DateTimePropertyRenderer(property);
            }
            else if (property.EditorType == PropertyGridHelperEditorType.CustomLookup)
            {
                renderer = new CustomLookupPropertyRenderer(property);
            }
            else if (property.EditorType == PropertyGridHelperEditorType.Percentage)
            {
                renderer = new PercentagePropertyRenderer(property);
            }

            return renderer;
        }

    }
}

