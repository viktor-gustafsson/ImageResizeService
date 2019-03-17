using SixLabors.Primitives;

namespace ImageResizeService.Controllers.Image.Models
{
    public class ImageResizeInputModel : ImageConversionBaseModel
    {
        public Size Size => new Size(Width,Height);
    }
}