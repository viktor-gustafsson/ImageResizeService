using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ImageResizeService.Infrastructure;
using ImageResizeService.Services.ImageProcessor.Models;
using Polly;
using Polly.Retry;
using SkiaSharp;

namespace ImageResizeService.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy _retryPolicy;

        public ImageService(HttpClientRetrySettings httpClientRetrySettings)
        {
            _httpClient = new HttpClient();

            _retryPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(httpClientRetrySettings.MaxRetryAttempts,
                    i => TimeSpan.FromMilliseconds(httpClientRetrySettings.TimeOutInMilliseconds));
        }
        
        public async Task<SKBitmap> GetImage(string url)
        {
            var imageAsBytes = await GetImageFromSourceAsStream(url);
            var skBitmap = SKBitmap.Decode(imageAsBytes);
            return skBitmap;
        }

        public async Task<ModifiedImage> SaveImage(SKSurface surface, SKEncodedImageFormat format)
        {
            switch (format)
            {
                case SKEncodedImageFormat.Jpeg:
                    return await SaveImageAsJpeg(surface);
                case SKEncodedImageFormat.Png:
                    return await SaveImageAsPng(surface);
                default:
                    throw new Exception();
            }
        }
        
        private async Task<ModifiedImage> SaveImageAsJpeg(SKSurface surface)
        {
            using (var encodedImage = surface.Snapshot())
            {
                return await Task.Run(() =>
                {
                    var data = encodedImage.Encode(SKEncodedImageFormat.Jpeg, 100);
                    return new ModifiedImage("image/jpeg", data.ToArray());
                });
            }
        }

        private async Task<ModifiedImage> SaveImageAsPng(SKSurface surface)
        {
            using (var encodedImage = surface.Snapshot())
            {
                return await Task.Run(() =>
                {
                    var data = encodedImage.Encode(SKEncodedImageFormat.Png, 0);
                    return new ModifiedImage("image/png", data.ToArray());
                });
            }
        }

        private async Task<byte[]> GetImageFromSourceAsStream(string imageUrl)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync(imageUrl);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(
                        $"Could not get image from {imageUrl}, status code: {response.StatusCode}");

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);
                    return memStream.ToArray();
                }
            });
        }
    }
}