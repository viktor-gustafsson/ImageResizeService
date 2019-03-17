using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using ImageResizeService.Controllers.Image.Models;
using ImageResizeService.Services.ImageProcessor.Models;
using ImageResizeService.Services.ImageService;

namespace ImageResizeService.Services.ImageProcessor
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly IImageService _imageService;

        public ImageProcessor(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<ModifiedImage> ReSizeImage(ImageResizeInputModel imageResizeInputModel)
        {
            var image = await _imageService.GetImage(imageResizeInputModel.Url);

            var destRect = new Rectangle(0, 0, imageResizeInputModel.Width, imageResizeInputModel.Height);
            var destImage = GetDestinationImage(imageResizeInputModel, image);

            using (var graphics = Graphics.FromImage(destImage))
            {
                SetImageProperties(graphics);

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return await _imageService.SaveImage(destImage, image.RawFormat.Guid);
        }

        public async Task<ModifiedImage> CropImage(ImageCropInputModel imageCropInputModel)
        {
            var image = await _imageService.GetImage(imageCropInputModel.Url);

            var destRect = new Rectangle(0, 0, imageCropInputModel.Width, imageCropInputModel.Height);
            var srcRect = new RectangleF(imageCropInputModel.X, imageCropInputModel.Y, imageCropInputModel.WidthToCrop,
                imageCropInputModel.HeightToCrop);

            var destImage = GetDestinationImage(imageCropInputModel, image);

            using (var graphics = Graphics.FromImage(destImage))
            {
                SetImageProperties(graphics);
                graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            }

            return await _imageService.SaveImage(destImage, image.RawFormat.Guid);
        }

        private static Bitmap GetDestinationImage(ImageConversionBaseModel imageCropInputModel, Image image)
        {
            var destImage = new Bitmap(image, imageCropInputModel.Width, imageCropInputModel.Height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            return destImage;
        }

        private static void SetImageProperties(Graphics graphics)
        {
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }
    }
}