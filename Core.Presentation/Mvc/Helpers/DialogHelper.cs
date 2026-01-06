using System.Text;
using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    public class DialogHelper : Helper
    {
        public DialogHelper(HtmlHelper htmlHelper) : base(htmlHelper) { }

        public MvcHtmlString DoDialog(DialogHelperData data)
        {
            return new MvcHtmlString(DoDialogHelper(data));
        }

        public static string DoDialogHelper(DialogHelperData data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class=\"modal");
            if (!string.IsNullOrWhiteSpace(data.Class))
                stringBuilder.Append(" ").Append(data.Class);
            stringBuilder.Append("\"");
            if (!string.IsNullOrWhiteSpace(data.Id))
                stringBuilder.Append(" id=\"").Append(data.Id).Append("\"");
            stringBuilder.Append(">\n");
            stringBuilder.Append("    <div class=\"modal-dialog modal-lg\">\n");
            stringBuilder.Append("        <div class=\"modal-content\">\n");
            stringBuilder.Append("            <div class=\"modal-header\">\n");

            if (data.IsCloseButtonVisible)
            {
                stringBuilder.Append("                <button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>\n");
            }

            stringBuilder.Append("                <h2 class=\"modal-title\">").Append(data.Title).Append("</h2>\n");
            stringBuilder.Append("            </div>\n");
            stringBuilder.Append("            <div class=\"modal-body\">\n");
            stringBuilder.Append(data.Content);
            stringBuilder.Append("            </div>\n");

            if (!data.IsFooterHidden)
            {
                stringBuilder.Append("            <div class=\"modal-footer\">\n");
                stringBuilder.Append("                <div class=\"btn-group\">\n");
                stringBuilder.Append("                    <a class=\"btn btn-default do-update\" title=\"").Append(data.UpdateButtonText).Append("\"><span class=\"glyphicon glyphicon-ok\"></span></a>\n");
                stringBuilder.Append("                    <a class=\"btn btn-default do-cancel\" title=\"").Append(data.CancelButtonText).Append("\"><span class=\"glyphicon glyphicon-remove\"></span></a>\n");
                stringBuilder.Append("                </div>\n");
                stringBuilder.Append("            </div>\n");
            }

            stringBuilder.Append("        </div>\n");
            stringBuilder.Append("    </div>\n");
            stringBuilder.Append("</div>\n");
            return stringBuilder.ToString();
        }

    }
}

