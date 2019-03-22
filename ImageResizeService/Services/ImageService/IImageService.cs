using System.Threading.Tasks;
using ImageResizeService.Services.ImageProcessor.Models;
using SkiaSharp;

namespace ImageResizeService.Services.ImageService
{
    public interface IImageService
    {
        Task<SKBitmap> GetImage(string url);
        Task<ModifiedImage> SaveImage(SKSurface surface, SKEncodedImageFormat format);
    }
}