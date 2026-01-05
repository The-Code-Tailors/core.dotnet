using System.Web;

namespace com.fabioscagliola.Core.DataAccess
{
    public class SessionCache
    {
        public static object GetValue(string key)
        {
            object value = null;

            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null)
            {
                value = HttpContext.Current.Session[key];
            }

            return value;
        }

        public static void SetValue(string key, object value)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = value;
            }
        }

        public static void FlushCache(string key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = null;
            }
        }

    }
}

