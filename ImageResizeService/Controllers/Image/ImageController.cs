using System;
using System.Threading.Tasks;
using ImageResizeService.Controllers.Image.Models;
using ImageResizeService.Services;
using ImageResizeService.Services.ImageProcessor;
using Microsoft.AspNetCore.Mvc;

namespace ImageResizeService.Controllers.Image
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageProcessor _imageProcessor;

        public ImageController(IImageProcessor imageProcessor)
        {
            _imageProcessor = imageProcessor;
        }

        [Route("resize")]
        [HttpGet]
        public async Task<IActionResult> ReSize([FromQuery] ImageResizeInputModel imageResizeInputModel)
        {
            var modifiedImage = await _imageProcessor.ReSizeImage(imageResizeInputModel);

            return File(modifiedImage.ImageAsBytes, modifiedImage.ImageFormat);
        }

        [Route("crop")]
        [HttpGet]
        public async Task<IActionResult> Crop([FromQuery] ImageCropInputModel imageCropInputModel)
        {
            try
            {
                var modifiedImage = await _imageProcessor.CropImage(imageCropInputModel);

                return File(modifiedImage.ImageAsBytes, modifiedImage.ImageFormat);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}