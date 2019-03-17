using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImageResizeService.Controllers.Image.Models;
using ImageResizeService.Infrastructure;
using ImageResizeService.Services.ImageProcessor.Models;
using Polly;
using Polly.Retry;

namespace ImageResizeService.Services.ImageProcessor
{
    public class Drawing : IImageProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy _retryPolicy;
        
        public Drawing(HttpClientRetrySettings httpClientRetrySettings)
        {
            _httpClient = new HttpClient();
            
            _retryPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(httpClientRetrySettings.MaxRetryAttempts,
                    i => TimeSpan.FromMilliseconds(httpClientRetrySettings.TimeOutInMilliseconds));
        }
        public async Task<ModifiedImage> ReSizeImage(ImageResizeInputModel imageResizeInputModel)
        {
            var imageAsStream = await GetImageFromSourceAsStream(imageResizeInputModel.Url);

            var image = Image.FromStream(imageAsStream);

            var destRect = new Rectangle(0, 0, imageResizeInputModel.Width, imageResizeInputModel.Height);
            var destImage = new Bitmap(image, imageResizeInputModel.Width, imageResizeInputModel.Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            var modifiedImage = await SaveImage(destImage, image.RawFormat.Guid);

            return modifiedImage;
        }

        public Task<ModifiedImage> CropImage(ImageCropInputModel imageCropInputModel)
        {
            throw new System.NotImplementedException();
        }
        
        private static async Task<ModifiedImage> SaveImage(Image image, Guid originalMimeTypeGuid)
        {
            return await Task.Run(() =>
            {
                using (var imageAsStream = new MemoryStream())
                {
                    image.Save(imageAsStream, image.RawFormat);

                    var mimeType = ImageCodecInfo.GetImageEncoders()
                        .First(info => info.FormatID == originalMimeTypeGuid).MimeType;
                    
                    return new ModifiedImage(mimeType,
                        imageAsStream.ToArray());
                }
            });
        }

        private async Task<Stream> GetImageFromSourceAsStream(string imageUrl)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync(imageUrl);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(
                        $"Could not get image from {imageUrl}, status code: {response.StatusCode}");

                return await response.Content.ReadAsStreamAsync();
            });
        }
    }
}