using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    public static class Extension
    {
        public static HelpersFactory Core(this HtmlHelper htmlHelper)
        {
            return new HelpersFactory(htmlHelper);
        }

    }
}

