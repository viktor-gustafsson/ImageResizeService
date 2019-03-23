using System.Threading.Tasks;
using ImageResizeService.Controllers.Image.Models;
using ImageResizeService.Services.ImageProcessor.Models;
using ImageResizeService.Services.ImageService;
using SkiaSharp;

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

            var info = new SKImageInfo(imageResizeInputModel.Width, imageResizeInputModel.Height);
            var encoding = imageResizeInputModel.GetEncoding();

            using (var surface = SKSurface.Create(info))
            {
                surface.Canvas.DrawBitmap(image, new SKRect(0, 0, image.Width, image.Height),
                    new SKRect(0, 0, imageResizeInputModel.Width, imageResizeInputModel.Height));

                return await _imageService.SaveImage(surface, encoding, imageResizeInputModel.JpegQuality);
            }
        }

        public async Task<ModifiedImage> CropImage(ImageCropInputModel imageCropInputModel)
        {
            var image = await _imageService.GetImage(imageCropInputModel.Url);

            var info = new SKImageInfo(imageCropInputModel.Width, imageCropInputModel.Height);
            var encoding = imageCropInputModel.GetEncoding();

            using (var surface = SKSurface.Create(info))
            {
                surface.Canvas.DrawBitmap(image,
                    new SKRect(imageCropInputModel.Left, imageCropInputModel.Top, imageCropInputModel.Right,
                        imageCropInputModel.Bottom),
                    new SKRect(0, 0, imageCropInputModel.Width, imageCropInputModel.Height));
                return await _imageService.SaveImage(surface, encoding);
            }
        }
    }
}