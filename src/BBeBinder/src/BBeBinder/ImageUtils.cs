using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace BBeBinder
{
    class ImageUtils
    {
        public static Bitmap[] SplitImage(Bitmap image, int width, int height)
        {
            // First we need to shrink the bitmap so that it is 600 across (keeping the height perspective
            float scale = (float)width / image.Width;
            int nheight = (int)(image.Height * scale);
            int nrImages = nheight / height;
            if (((float)nheight / height) - nrImages > 0)
                nrImages++;
            Bitmap[] retImages = new Bitmap[nrImages];
            Bitmap nimage = FixedSize(image, width, nheight);

            int rem = nheight;
            int pos = 0;
            int i = 0;
            while (rem > 0)
            {
                int amount = height;
                if (rem < height)
                    amount = rem;

                Bitmap bmPhoto = new Bitmap(width, amount, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(nimage.HorizontalResolution, image.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                grPhoto.DrawImage(nimage,
                    new Rectangle(0, 0, width, amount),
                    new Rectangle(0, pos, width, amount),
                    GraphicsUnit.Pixel);

                grPhoto.Dispose();
                retImages[i++] = bmPhoto;

                pos += amount;
                rem -= height;
            }

            return retImages;
        }

        private static Bitmap FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
}
