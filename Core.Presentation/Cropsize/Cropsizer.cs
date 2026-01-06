using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace com.fabioscagliola.Core.Presentation.Cropsize
{
    /// <summary>
    /// 
    /// </summary>
    public class Cropsizer
    {
        public enum Anchor
        {
            Center, // Default
            Top,
            Right,
            Bottom,
            Left,
        }

        /// <summary>
        /// Cropsizes an image
        /// </summary>
        /// <param name="bytes">The image</param>
        /// <param name="w">The target width in pixels</param>
        /// <param name="h">The target height in pixels</param>
        /// <param name="anchor"></param>
        public byte[] Cropsize(byte[] bytes, int w, int h, Anchor anchor)
        {
            using var image = Image.Load(bytes);

            var w0 = image.Width;
            var h0 = image.Height;

            var r1 = (double)w0 / h0;
            var r2 = (double)w / h;

            var w1 = w0;
            var h1 = h0;

            if (r1 < r2) // Source is wide 
            {
                h1 = (int)((double)w1 / w * h);
            }

            if (r1 > r2) // Source is tall 
            {
                w1 = (int)((double)h1 / h * w);
            }
            //else  // Source is square 
            //{
            //    // Do nothing 
            //}

            var x = (w0 - w1) / 2;
            var y = (h0 - h1) / 2;

            switch (anchor)
            {
                case Anchor.Top:
                    y = 0;
                    break;
                case Anchor.Right:
                    x = w0 - w1;
                    break;
                case Anchor.Bottom:
                    y = h0 - h1;
                    break;
                case Anchor.Left:
                    x = 0;
                    break;
                case Anchor.Center:
                default:
                    // Do nothing
                    break;
            }

            image.Mutate(imageProcessingContext =>
            {
                imageProcessingContext.Crop(new Rectangle(x, y, w1, h1));
                imageProcessingContext.Resize(w, h);
            });

            using var memoryStream = new MemoryStream();
            image.SaveAsPng(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Resizes an image and compresses it using the JPEG method 
        /// </summary>
        public byte[] ResizeJpeg(byte[] bytes, int w, int h, bool preserve, int quality)
        {
            using var image = Image.Load(bytes);
            var w0 = image.Width;
            var h0 = image.Height;

            var w1 = w;
            var h1 = h;

            if (preserve)
            {
                var rw = (double)w0 / w;
                var rh = (double)h0 / h;

                var r = rw;

                if (rh > rw)
                {
                    r = rh;
                }

                w1 = (int)((double)w0 / r);
                h1 = (int)((double)h0 / r);
            }

            image.Mutate(imageProcessingContext =>
            {
                imageProcessingContext.Resize(w1, h1);
            });

            using var memoryStream = new MemoryStream();
            image.SaveAsJpeg(memoryStream, new JpegEncoder() { Quality = quality });
            return memoryStream.ToArray();
        }
    }
}