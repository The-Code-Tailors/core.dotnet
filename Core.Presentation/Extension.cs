using System.Web.UI;

namespace com.fabioscagliola.Core.Presentation
{
    public static class Extension
    {
        public static Control FindControlRecursively(this Control _this, string id)
        {
            Control control = null;
            if (_this.ID == id)
            {
                control = _this;
            }
            else
            {
                foreach (Control childControl in _this.Controls)
                {
                    control = childControl.FindControlRecursively(id);
                    if (control != null)
                    {
                        break;
                    }
                }
            }
            return control;
        }

    }
}

