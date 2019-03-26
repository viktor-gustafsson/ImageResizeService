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

            var info = GetImageInfo(imageResizeInputModel);
            var encoding = imageResizeInputModel.GetEncoding();

            using (var surface = SKSurface.Create(info))
            {
                surface.Canvas.DrawBitmap(image, new SKRect(0, 0, image.Width, image.Height),
                    GetDestinationRectangle(imageResizeInputModel));

                return await _imageService.SaveImage(surface, encoding, imageResizeInputModel.JpegQuality);
            }
        }

        public async Task<ModifiedImage> CropImage(ImageCropInputModel imageCropInputModel)
        {
            var image = await _imageService.GetImage(imageCropInputModel.Url);

            var info = GetImageInfo(imageCropInputModel);
            var encoding = imageCropInputModel.GetEncoding();

            using (var surface = SKSurface.Create(info))
            {
                surface.Canvas.DrawBitmap(image,
                    new SKRect(imageCropInputModel.Left, imageCropInputModel.Top, imageCropInputModel.Right,
                        imageCropInputModel.Bottom), GetDestinationRectangle(imageCropInputModel));
                return await _imageService.SaveImage(surface, encoding);
            }
        }

        private static SKRect GetDestinationRectangle(ImageConversionBaseModel inputModel) =>
            new SKRect(0, 0, inputModel.Width, inputModel.Height);

        private static SKImageInfo GetImageInfo(ImageConversionBaseModel inputModel) =>
            new SKImageInfo(inputModel.Width, inputModel.Height);
    }
}