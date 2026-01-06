using com.fabioscagliola.Core.DataAccess;
using com.fabioscagliola.Core.DataAccess.Social;
using com.fabioscagliola.Core.Presentation.Cropsize;
using System;
using System.Web;
using System.Web.SessionState;

namespace com.fabioscagliola.Core.Presentation.Handlers
{
    public class BlobHandler : IHttpHandler, IRequiresSessionState
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

                Blob blob = Blob.Select(milieu, guid);

                int w, h;

                int.TryParse(context.Request["Width"], out w);
                int.TryParse(context.Request["Height"], out h);

                if (w != 0 && h != 0 && (blob.ContentType == "image/jpeg" || blob.ContentType == "image/png"))
                {
                    // Default to max 
                    int w1 = Blobfier.MAX;
                    int h1 = Blobfier.MAX;

                    // Ensure specified target dimensions do not exceed max dimensions 

                    if (w > Blobfier.MAX || h > Blobfier.MAX)
                    {
                        // Do nothing, default to max 
                    }
                    else
                    {
                        w1 = w;
                        h1 = h;
                    }

                    Cropsizer cropsizer = new Cropsizer();
                    byte[] bytes = cropsizer.Cropsize(blob.Content, w1, h1);

                    context.Response.AppendHeader("Content-Length", bytes.Length.ToString());
                    context.Response.ContentType = blob.ContentType;
                    context.Response.BinaryWrite(bytes);
                }
                else
                {
                    context.Response.AppendHeader("Content-Disposition", string.Concat("attachment; filename=", blob.Name));
                    context.Response.AppendHeader("Content-Length", blob.ContentLength.ToString());
                    context.Response.ContentType = blob.ContentType;
                    context.Response.BinaryWrite(blob.Content);
                }

                //context.Response.End();
            }
            catch (Exception e)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(e.ToString());
            }
        }

    }
}

