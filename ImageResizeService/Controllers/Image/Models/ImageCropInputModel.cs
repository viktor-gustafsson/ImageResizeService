using System.ComponentModel.DataAnnotations;
using ImageResizeService.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SixLabors.Primitives;

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
        
        public Point Point => new Point(X, Y);
        public Size Size => new Size(Width, Height);
    }
}