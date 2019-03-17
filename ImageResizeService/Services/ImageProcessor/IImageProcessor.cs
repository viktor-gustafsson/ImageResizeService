using System.Threading.Tasks;
using ImageResizeService.Controllers.Image.Models;
using ImageResizeService.Services.ImageProcessor.Models;

namespace ImageResizeService.Services.ImageProcessor
{
    public interface IImageProcessor
    {
        Task<ModifiedImage> ReSizeImage(ImageResizeInputModel imageResizeInputModel);
        Task<ModifiedImage> CropImage(ImageCropInputModel imageCropInputModel);
    }
}