using SkiaSharp;

namespace ImageResizeService.Controllers.Image.Models
{
    public class ImageResizeInputModel : ImageConversionBaseModel
    {
        public SKSize Size => new SKSize(Width,Height);
    }
}