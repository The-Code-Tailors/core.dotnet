using com.fabioscagliola.Core.DataAccess;
using System.Collections.Generic;
using System.Web;

namespace com.fabioscagliola.Core.Presentation
{
    public static class SessionWrapper
    {
        private static readonly string ACTIVEDOMAINID = "com.fabioscagliola.Core.Presentation.SessionWrapper.ActiveDomainId";
        private static readonly string USERDOMAINLIST = "com.fabioscagliola.Core.Presentation.SessionWrapper.UserDomainList";

        public static long? ActiveDomainId
        {
            get
            {
                long? value = null;

                if (HttpContext.Current.Session[ACTIVEDOMAINID] != null)
                {
                    value = (long)HttpContext.Current.Session[ACTIVEDOMAINID];
                }

                return value;
            }
            set
            {
                HttpContext.Current.Session[ACTIVEDOMAINID] = value;
            }
        }

        public static List<Domain> UserDomainList
        {
            get
            {
                List<Domain> value = null;

                if (HttpContext.Current.Session[USERDOMAINLIST] != null)
                {
                    value = (List<Domain>)HttpContext.Current.Session[USERDOMAINLIST];
                }

                return value;
            }
            set
            {
                HttpContext.Current.Session[USERDOMAINLIST] = value;
            }
        }

    }
}

