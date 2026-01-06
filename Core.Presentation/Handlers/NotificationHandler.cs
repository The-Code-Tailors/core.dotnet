using com.fabioscagliola.Core.DataAccess;
using com.fabioscagliola.Core.DataAccess.Social;
using System;
using System.Web;
using System.Web.SessionState;

namespace com.fabioscagliola.Core.Presentation.Handlers
{
    public abstract class NotificationHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                Guid guid;

                if (!Guid.TryParse(context.Request["Guid"], out guid))
                {
                    throw new PresentationException("GUID expected!");
                }

                User user = User.Select(Milieu.SystemMilieu, context.User.Identity.Name);
                Domain domain = Util.SelectActiveDomain(user);
                Milieu milieu = new Milieu(domain.Id, user.Id);

                Notification notification = Notification.Select(milieu, guid);
                string url = GetUrl(notification);
                context.Response.Redirect(url, true);
            }
            catch (Exception e)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(e.ToString());
            }
        }

        protected abstract string GetUrl(Notification notification);

    }
}

