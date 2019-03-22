using System.ComponentModel.DataAnnotations;
using ImageResizeService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace ImageResizeService.Controllers.Image.Models
{
    public class ImageCropInputModel : ImageConversionBaseModel
    {
        [Required]
        [SizeValidator]
        [FromQuery(Name = "x")]
        public int X { get; set; }

        [Required]
        [SizeValidator]
        [FromQuery(Name = "y")]
        public int Y { get; set; }
        
        [Required]
        [SizeValidator]
        [FromQuery(Name = "widthtocrop")]
        public int WidthToCrop { get; set; }   
        
        [Required]
        [SizeValidator]
        [FromQuery(Name = "heighttocrop")]
        public int HeightToCrop { get; set; }
        
        public SKPoint Point => new SKPoint(X, Y);
        public SKSize Size => new SKSize(Width, Height);
    }
}