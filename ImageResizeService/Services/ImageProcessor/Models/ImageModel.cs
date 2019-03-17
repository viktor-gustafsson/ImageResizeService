using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace ImageResizeService.Services.ImageProcessor.Models
{
    public class ImageModel
    {
        public Image<Rgba32> Image { get; }
        public IImageFormat ImageFormat { get; }

        public ImageModel(Image<Rgba32> image, IImageFormat imageFormat)
        {
            Image = image;
            ImageFormat = imageFormat;
        }
    }
}