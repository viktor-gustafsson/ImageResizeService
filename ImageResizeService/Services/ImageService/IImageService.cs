using System;
using System.Drawing;
using System.Threading.Tasks;
using ImageResizeService.Services.ImageProcessor.Models;

namespace ImageResizeService.Services.ImageService
{
    public interface IImageService
    {
        Task<Image> GetImage(string url);
        Task<ModifiedImage> SaveImage(Image image, Guid originalMimeTypeGuid);
    }
}