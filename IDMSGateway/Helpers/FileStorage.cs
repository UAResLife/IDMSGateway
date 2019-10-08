using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace CatCardGateway.Helpers
{
    public class LocalStorage
    {
        /// <summary>
        /// The path in the hard drive where the photos are going to be stored.
        /// </summary>
        public string PhotosPath { get; set; }

        /// <summary>
        /// Evaluates if the picture exists in the hard drive.
        /// </summary>
        /// <param name="PhotoName">The full name of the picture (including extension)</param>
        /// <returns>A Bool Value</returns>
        public bool PhotoExists(String PhotoName)
        {
            //Verify if the path exists
            var d = new DirectoryInfo(PhotosPath);
            //if it does not exists, create it.
            if (!d.Exists) d.Create();
            
            return File.Exists(Path.Combine(PhotosPath, PhotoName));
        }

        public FileInfo GetLocalPhotoInfo(string PhotoName)
        {
            return new FileInfo(Path.Combine(PhotosPath, PhotoName));
        }

        /// <summary>
        /// Returns the picture from the Hard Drive
        /// </summary>
        /// <param name="PhotoName">The name of the picture to be returned</param>
        /// <returns></returns>
        public Image GetPhoto(String PhotoName)
        {
            var img = Image.FromFile(Path.Combine(PhotosPath, PhotoName));
            return img;
        }

        /// <summary>
        /// Saves a shrinked copy (248 by 330) version of the photo to the Hard Drive
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="PhotoName"></param>
        public void SavePhoto(byte[] photo, String PhotoName)
        {
            var ms = new MemoryStream(photo);
            var img = Image.FromStream(ms);
            var resizedImg = ResizePhoto(img, 248, 330);
            resizedImg.Save(Path.Combine(PhotosPath,PhotoName));
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// https://stackoverflow.com/questions/1922040/how-to-resize-an-image-c-sharp
        /// </summary>
        /// <param name="photo">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizePhoto(Image photo, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destPhoto = new Bitmap(width, height);

            destPhoto.SetResolution(photo.HorizontalResolution, photo.VerticalResolution);

            using (var graphics = Graphics.FromImage(destPhoto))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(photo, destRect, 0, 0, photo.Width, photo.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destPhoto;
        }
    }
}
