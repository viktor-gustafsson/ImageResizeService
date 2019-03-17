using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImageResizeService.Infrastructure;
using ImageResizeService.Services.ImageProcessor.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        
        public async Task<ModifiedImage> SaveImage(SKBitmap image, Guid originalMimeTypeGuid)
        {
            throw new NotImplementedException();
            
//            return await Task.Run(() =>
//            {
//                using (var imageAsStream = new MemoryStream())
//                {
//                    image.Save(imageAsStream, image.RawFormat);
//
//                    var mimeType = ImageCodecInfo.GetImageEncoders()
//                        .First(info => info.FormatID == originalMimeTypeGuid).MimeType;
//
//                    return new ModifiedImage(mimeType,
//                        imageAsStream.ToArray());
//                }
//            });
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