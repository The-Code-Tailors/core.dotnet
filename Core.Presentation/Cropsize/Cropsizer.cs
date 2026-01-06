using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace com.fabioscagliola.Core.Presentation.Cropsize
{
    /// <summary>
    /// 
    /// </summary>
    public class Cropsizer
    {
        public enum Anchor
        {
            Center,  // Default
            Top,
            Right,
            Bottom,
            Left,

        }

        public class ImageCropsizedEventArgs : EventArgs
        {
            public string File { get; set; }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ImageCropsizedHandler(object sender, ImageCropsizedEventArgs e);

        /// <summary>
        /// Raised after an image has been cropsized
        /// </summary>
        public event ImageCropsizedHandler ImageCropsized;

        /// <summary>
        /// Cropsizes an image with anchor = center
        /// </summary>
        public byte[] Cropsize(byte[] bytes, int w, int h)
        {
            return Cropsize(bytes, w, h, Anchor.Center);
        }

        /// <summary>
        /// Cropsizes an image
        /// </summary>
        public byte[] Cropsize(byte[] bytes, int w, int h, Anchor anchor)
        {
            MemoryStream memoryStream = new MemoryStream(bytes);
            Bitmap bitmap = new Bitmap(memoryStream);

            try
            {
                double r1 = (double)bitmap.Width / bitmap.Height;
                double r2 = (double)w / h;

                int w1 = bitmap.Width;
                int h1 = bitmap.Height;

                if (r1 < r2)  // Source is wide 
                {
                    h1 = (int)((double)w1 / w * h);
                }
                if (r1 > r2)  // Source is tall 
                {
                    w1 = (int)((double)h1 / h * w);
                }
                //else  // Source is square 
                //{
                //    // Do nothing 
                //}

                int x = (bitmap.Width - w1) / 2;
                int y = (bitmap.Height - h1) / 2;

                switch (anchor)
                {
                    case Anchor.Top:
                        y = 0;
                        break;
                    case Anchor.Right:
                        x = bitmap.Width - w1;
                        break;
                    case Anchor.Bottom:
                        y = bitmap.Height - h1;
                        break;
                    case Anchor.Left:
                        x = 0;
                        break;
                    case Anchor.Center:
                    default:
                        // Do nothing
                        break;
                }

                return Resize(bitmap, w1, h1, w, h, x, y);
            }
            finally
            {
                memoryStream.Dispose();
                bitmap.Dispose();
            }
        }

        /// <summary>
        /// Cropsizes an image
        /// </summary>
        /// <param name="path">The full path to the image</param>
        /// <param name="w">The target width in pixels</param>
        /// <param name="h">The target height in pixels</param>
        /// <param name="folder">The full path to the destination folder</param>
        /// <param name="anchor"></param>
        public void Cropsize(string path, int w, int h, string folder, Anchor anchor)  // TODO: Reengineer to use the overload above 
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            using (Bitmap source = new Bitmap(path))
            {
                double r1 = (double)source.Width / source.Height;
                double r2 = (double)w / h;

                int w1 = source.Width;
                int h1 = source.Height;

                if (r1 < r2)
                {
                    h1 = (int)((double)w1 / w * h);
                }
                if (r1 > r2)
                {
                    w1 = (int)((double)h1 / h * w);
                }

                int x = (source.Width - w1) / 2;
                int y = (source.Height - h1) / 2;

                switch (anchor)
                {
                    case Anchor.Top:
                        y = 0;
                        break;
                    case Anchor.Right:
                        x = source.Width - w1;
                        break;
                    case Anchor.Bottom:
                        y = source.Height - h1;
                        break;
                    case Anchor.Left:
                        x = 0;
                        break;
                    case Anchor.Center:
                    default:
                        // Do nothing
                        break;
                }

                Rectangle sourceRectangle = new Rectangle(x, y, w1, h1);
                Rectangle targetRectangle = new Rectangle(0, 0, w, h);

                using (Bitmap target = new Bitmap(w, h))
                {
                    Graphics graphics = Graphics.FromImage(target);
                    graphics.DrawImage(source, targetRectangle, sourceRectangle, GraphicsUnit.Pixel);
                    string targetFilename = Path.Combine(folder, string.Format("{0}_{1}x{2}.jpg", Path.GetFileNameWithoutExtension(path), w, h));
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                    target.Save(targetFilename, GetImageCodecInfo(ImageFormat.Jpeg), encoderParameters);
                    OnImageCropsized(new ImageCropsizedEventArgs() { File = path, });
                }
            }
        }

        /// <summary>
        /// Cropsizes all the images in a folder
        /// </summary>
        /// <param name="path">The full path to the folder</param>
        /// <param name="w">The target width in pixels</param>
        /// <param name="h">The target height in pixels</param>
        /// <param name="folder">The full path to the destination folder</param>
        /// <param name="anchor"></param>
        public void CropsizeFolder(string path, int w, int h, string folder, Anchor anchor)
        {
            foreach (string file in Directory.GetFiles(path))
            {
                try
                {
                    Cropsize(file, w, h, folder, anchor);
                }
                catch (Exception e)
                {
                    Trace.WriteLine(string.Format("The following error occurred while cropsizing image \"{0}\".", file));
                    Trace.WriteLine(e);
                }
            }
        }

        /// <summary>
        /// Resizes an image
        /// </summary>
        public byte[] Resize(byte[] bytes, int w, int h)
        {
            return Resize(bytes, w, h, false);
        }

        /// <summary>
        /// Resizes an image
        /// </summary>
        public byte[] Resize(byte[] bytes, int w, int h, bool preserve)
        {
            MemoryStream sourceMemoryStream = new MemoryStream(bytes);
            Bitmap bitmap = new Bitmap(sourceMemoryStream);

            try
            {
                int sw = bitmap.Width;
                int sh = bitmap.Height;

                DoWidthAndHeight(w, h, sw, sh, preserve, out int w1, out int h1);

                return Resize(bitmap, sw, sh, w1, h1);
            }
            finally
            {
                sourceMemoryStream.Dispose();
                bitmap.Dispose();
            }
        }

        /// <summary>
        /// Resizes an image and compresses it using the JPEG method 
        /// </summary>
        public byte[] ResizeJpeg(byte[] bytes, int w, int h, bool preserve, long quality)
        {
            MemoryStream sourceMemoryStream = new MemoryStream(bytes);
            Bitmap bitmap = new Bitmap(sourceMemoryStream);

            try
            {
                int sw = bitmap.Width;
                int sh = bitmap.Height;

                DoWidthAndHeight(w, h, sw, sh, preserve, out int w1, out int h1);

                return ResizeJpeg(bitmap, sw, sh, w1, h1, 0, 0, quality);
            }
            finally
            {
                sourceMemoryStream.Dispose();
                bitmap.Dispose();
            }
        }

        #region Protected members

        protected Bitmap DoResize(Bitmap source, int sourceWidth, int sourceHeight, int targetWidth, int targetHeight, int x, int y)
        {
            Rectangle sourceRectangle = new Rectangle(x, y, sourceWidth, sourceHeight);
            Rectangle targetRectangle = new Rectangle(0, 0, targetWidth, targetHeight);
            Bitmap target = new Bitmap(targetWidth, targetHeight);
            Graphics graphics = Graphics.FromImage(target);
            graphics.DrawImage(source, targetRectangle, sourceRectangle, GraphicsUnit.Pixel);
            return target;
        }

        protected void DoWidthAndHeight(int w, int h, int sw, int sh, bool preserve, out int w1, out int h1)
        {
            w1 = w;
            h1 = h;

            if (preserve)
            {
                double rw = (double)sw / w;
                double rh = (double)sh / h;

                double r = rw;

                if (rh > rw)
                {
                    r = rh;
                }

                w1 = (int)((double)sw / r);
                h1 = (int)((double)sh / r);
            }
        }

        protected ImageCodecInfo GetImageCodecInfo(ImageFormat imageFormat)
        {
            foreach (ImageCodecInfo imageCodecInfo in ImageCodecInfo.GetImageDecoders())
            {
                if (imageCodecInfo.FormatID == imageFormat.Guid)
                {
                    return imageCodecInfo;
                }
            }

            throw new ApplicationException("ImageCodecInfo not found!");
        }

        protected void OnImageCropsized(ImageCropsizedEventArgs e)
        {
            if (ImageCropsized != null)
            {
                ImageCropsized(this, e);
            }
        }

        protected byte[] Resize(Bitmap source, int sourceWidth, int sourceHeight, int targetWidth, int targetHeight)
        {
            return Resize(source, sourceWidth, sourceHeight, targetWidth, targetHeight, 0, 0);
        }

        protected byte[] Resize(Bitmap source, int sourceWidth, int sourceHeight, int targetWidth, int targetHeight, int x, int y)
        {
            MemoryStream memoryStream = new MemoryStream();

            Bitmap target = DoResize(source, sourceWidth, sourceHeight, targetWidth, targetHeight, x, y);

            try
            {
                target.Save(memoryStream, ImageFormat.Png);
                return memoryStream.ToArray();
            }
            finally
            {
                memoryStream.Dispose();
                target.Dispose();
            }
        }

        protected byte[] ResizeJpeg(Bitmap source, int sourceWidth, int sourceHeight, int targetWidth, int targetHeight, int x, int y, long quality)
        {
            MemoryStream memoryStream = new MemoryStream();

            Bitmap target = DoResize(source, sourceWidth, sourceHeight, targetWidth, targetHeight, x, y);

            try
            {
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                target.Save(memoryStream, GetImageCodecInfo(ImageFormat.Jpeg), encoderParameters);
                return memoryStream.ToArray();
            }
            finally
            {
                memoryStream.Dispose();
                target.Dispose();
            }
        }

        #endregion

    }
}

