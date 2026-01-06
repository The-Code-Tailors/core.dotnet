using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace com.fabioscagliola.Core.Presentation
{
    public static class ImageStacker
    {
        public static byte[] Do(params byte[][] images)
        {
            if (images.Length == 0)
            {
                throw new PresentationException("No images!");
            }
            Bitmap bitmap;
            using (MemoryStream background = new MemoryStream(images[0]))
            {
                bitmap = new Bitmap(background);
                Graphics graphics = Graphics.FromImage(bitmap);
                if (images.Length != 1)
                {
                    for (int i = 1; i < images.Length; i++)
                    {
                        using (MemoryStream memoryStream = new MemoryStream(images[i]))
                        {
                            graphics.DrawImage(new Bitmap(memoryStream), new Point(0, 0));
                        }
                    }
                }
            }
            using (MemoryStream target = new MemoryStream())
            {
                bitmap.Save(target, ImageFormat.Png);
                return target.ToArray();
            }
        }

    }
}

