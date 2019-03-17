using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImageResizeService.Infrastructure;
using ImageResizeService.Services.ImageProcessor.Models;
using Polly;
using Polly.Retry;

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
        
        public async Task<Image> GetImage(string url)
        {
            var imageAsStream = await GetImageFromSourceAsStream(url);
            var image = Image.FromStream(imageAsStream);
            return image;
        }
        
        public async Task<ModifiedImage> SaveImage(Image image, Guid originalMimeTypeGuid)
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