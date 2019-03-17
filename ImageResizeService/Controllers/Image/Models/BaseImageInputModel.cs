using System.ComponentModel.DataAnnotations;
using ImageResizeService.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ImageResizeService.Controllers.Image.Models
{
    public class ImageConversionBaseModel
    {
        [Required]
        [Url]
        [FromQuery(Name = "url")] 
        public string Url { get; set; }
        
        [Required]
        [SizeValidator]
        [FromQuery(Name = "height")] 
        public int Height { get; set; }
        
        [Required]
        [SizeValidator]
        [FromQuery(Name = "width")] 
        public int Width { get; set; }
    }
}