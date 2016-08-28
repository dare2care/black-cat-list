namespace BlackCatList.Web.Models
{
    using System.IO;
    using System.Web;
    using ImageProcessor;
    using ImageProcessor.Imaging;

    public static class EntityExtensions
    {
        private static ResizeLayer resizeLayer
            = new ResizeLayer(new System.Drawing.Size(256, 256), ResizeMode.Max, upscale: false);

        public static void SaveImage(this IImageEntity entity, HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var imageFactory = new ImageFactory())
                {
                    imageFactory.Load(file.InputStream)
                        .Resize(resizeLayer)
                        .Quality(70)
                        .Save(memoryStream);
                }

                entity.Image = new Image
                {
                    Name = file.FileName,
                    Content = memoryStream.ToArray(),
                    ContentType = file.ContentType
                };
            }
        }
    }
}