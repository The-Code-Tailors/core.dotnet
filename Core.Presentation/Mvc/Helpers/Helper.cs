using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    public abstract class Helper
    {
        protected HtmlHelper htmlHelper;

        public Helper(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

    }
}

