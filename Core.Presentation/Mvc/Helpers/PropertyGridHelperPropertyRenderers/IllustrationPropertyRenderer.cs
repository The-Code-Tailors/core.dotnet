using System;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers.PropertyGridHelperPropertyRenderers
{
    public class IllustrationPropertyRenderer : PropertyRenderer
    {
        public IllustrationPropertyRenderer(PropertyGridHelperProperty property) : base(property) { }

        public override string RenderEditor(FormHelper formHelper)
        {
            if (property.IsReadOnly)
            {
                return null;
            }
            else
            {
                return formHelper.DoFileHelper(property.Identifier, property.Label, property.Description);
            }
        }

        public override string RenderViewer()
        {
            string result = null;

            if (property.Value != null && (Guid)property.Value != default(Guid))
            {
                result = $"<img class=\"img-responsive\" src=\"/images/{property.Value}\" />";
            }

            return result;
        }

    }
}

