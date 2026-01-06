using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc.Helpers
{
    public class HelpersFactory
    {
        protected HtmlHelper htmlHelper;

        public HelpersFactory(HtmlHelper htmlHelper)
        {
            this.htmlHelper = htmlHelper;
        }

        public DialogHelper Dialog()
        {
            return new DialogHelper(htmlHelper);
        }

        public FormHelper Form()
        {
            return new FormHelper(htmlHelper);
        }

        public GridHelper Grid()
        {
            return new GridHelper(htmlHelper);
        }

        public PropertyGridHelper PropertyGrid()
        {
            return new PropertyGridHelper(htmlHelper);
        }

    }
}

