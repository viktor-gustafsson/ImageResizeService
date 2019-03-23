using System;
using System.ComponentModel.DataAnnotations;
using ImageResizeService.Infrastructure.Validators;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

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

        [Required]
        [ImageFormatValidator]
        [FromQuery(Name = "imageFormat")]
        public string ImageFormat { get; set; }

        [QualityValidator]
        [FromQuery(Name = "jpegQuality")]
        public int JpegQuality { get; set; }

        public SKEncodedImageFormat GetEncoding()
        {
            switch (ImageFormat.ToLower())
            {
                case "jpeg":
                    return SKEncodedImageFormat.Jpeg;
                case "png":
                    return SKEncodedImageFormat.Png;

                default:
                    throw new InvalidOperationException("Could not map format string to SkEncodedImageFormat");
            }
        }
    }
}