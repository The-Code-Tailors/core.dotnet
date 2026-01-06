using com.fabioscagliola.Core.DataAccess;
using com.fabioscagliola.Core.DataAccess.Social;
using com.fabioscagliola.Core.Presentation.Cropsize;
using System;
using System.Web.Mvc;

namespace com.fabioscagliola.Core.Presentation.Mvc
{
    public class BlobController : CoreController
    {
        public ActionResult Index(Guid guid, int width, int height)
        {
            Blob blob = Blob.Select(Milieu.SystemMilieu, guid);  // System milieu 'cause everyone must be able to access images 

            if (blob.Id != 0)
            {
                return Image(blob.Content, width, height);
            }

            return new HttpNotFoundResult();
        }

        protected ActionResult Image(byte[] bytes, int width, int height)
        {
            // Default to max 
            int w = Blobfier.MAX;
            int h = Blobfier.MAX;

            // Ensure specified target dimensions do not exceed max dimensions 

            if (width > Blobfier.MAX || height > Blobfier.MAX)
            {
                // Do nothing, default to max 
            }
            else
            {
                w = width;
                h = height;
            }

            Cropsizer cropsizer = new Cropsizer();

            byte[] fileContents = cropsizer.Cropsize(bytes, w, h);

            return new FileContentResult(fileContents, "image/png");
        }

    }
}

