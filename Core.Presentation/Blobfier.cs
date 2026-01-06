using com.fabioscagliola.Core.DataAccess;
using com.fabioscagliola.Core.DataAccess.Social;
using com.fabioscagliola.Core.Presentation.Cropsize;
using System;
using System.Drawing;
using System.IO;
using System.Net;

namespace com.fabioscagliola.Core.Presentation
{
    public static class Blobfier
    {
        public const int MAX = 1920;

        public static Guid Blobfy(string imageUrl, DataAccessEntity entity)
        {
            return Blobfy(imageUrl, entity, Guid.Empty);
        }

        public static Guid Blobfy(string imageUrl, DataAccessEntity entity, Guid guid)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.UseDefaultCredentials = true;

                byte[] bytes = webClient.DownloadData(imageUrl);
                //string contentType = webClient.ResponseHeaders[HttpResponseHeader.ContentType];
                string name = Path.GetFileName(imageUrl);

                return Blobfy(bytes, name, entity, guid);
            }
        }

        public static Guid Blobfy(byte[] bytes, string name, DataAccessEntity entity)
        {
            return Blobfy(bytes, name, entity, Guid.Empty);
        }

        public static Guid Blobfy(byte[] bytes, string name, DataAccessEntity entity, Guid guid)
        {
            return BlobfyInternal(bytes, name, null, null, entity, guid);
        }

        public static Guid Blobfy(byte[] bytes, string name, int w, int h, DataAccessEntity entity, Guid guid)
        {
            return BlobfyInternal(bytes, name, w, h, entity, guid);
        }

        private static Guid BlobfyInternal(byte[] bytes, string name, int? w, int? h, DataAccessEntity entity, Guid guid)
        {
            // Default to max 
            int w1 = MAX;
            int h1 = MAX;

            if (w.HasValue && h.HasValue)
            {
                // Ensure specified target dimensions do not exceed max dimensions 

                if (w > MAX || h > MAX)
                {
                    // Do nothing, default to max 
                }
                else
                {
                    w1 = w.Value;
                    h1 = h.Value;
                }
            }
            else
            {
                // Ensure source dimensions do not exceed max dimensions 

                MemoryStream sourceMemoryStream = new MemoryStream(bytes);

                try
                {
                    Bitmap source = new Bitmap(sourceMemoryStream);

                    if (source.Width > MAX || source.Height > MAX)
                    {
                        // Do nothing, default to max 
                    }
                    else
                    {
                        w1 = source.Width;
                        h1 = source.Height;
                    }
                }
                finally
                {
                    sourceMemoryStream.Dispose();
                }
            }

            // Resize anyway in order to get a png 
            Cropsizer cropsizer = new Cropsizer();
            bytes = cropsizer.Resize(bytes, w1, h1, true);

            Blob blob = null;

            if (guid != Guid.Empty)
            {
                blob = Blob.Select(Milieu.SystemMilieu, guid);  // System milieu is ok 

                if (blob.Id == 0)
                {
                    throw new PresentationException("Existing blob not found!");
                }
            }
            else
            {
                blob = new Blob();
            }

            blob.Content = bytes;
            blob.ContentType = "image/png";
            blob.IsCompressed = false;
            blob.MasterEntity = entity.GetType().FullName;
            blob.MasterGuid = entity.Guid;
            blob.MasterId = entity.Id;
            blob.Name = name;

            blob.Update(Milieu.SystemMilieu);  // System milieu is ok 

            return blob.Guid;
        }

    }
}

