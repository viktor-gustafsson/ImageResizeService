using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ImageResizeService.Controllers.Image.Models;
using ImageResizeService.Infrastructure;
using ImageResizeService.Services.ImageProcessor.Models;
using Polly;
using Polly.Retry;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace ImageResizeService.Services.ImageProcessor
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy _retryPolicy;

        public ImageProcessor(HttpClientRetrySettings httpClientRetrySettings)
        {
            _httpClient = new HttpClient();

            _retryPolicy = Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(httpClientRetrySettings.MaxRetryAttempts,
                    i => TimeSpan.FromMilliseconds(httpClientRetrySettings.TimeOutInMilliseconds));
        }

        public async Task<ModifiedImage> ReSizeImage(ImageResizeInputModel imageResizeInputModel)
        {
            var imageModel = await GetImage(imageResizeInputModel.Url);

            imageModel.Image.Mutate(context => context.Resize(imageResizeInputModel.Size));

            return SaveImage(imageModel);
        }

        public async Task<ModifiedImage> CropImage(ImageCropInputModel imageCropInputModel)
        {
            var imageModel = await GetImage(imageCropInputModel.Url);

            var cropInstructions = new Rectangle(imageCropInputModel.Point, imageCropInputModel.Size);

            imageModel.Image.Mutate(context => context.Crop(cropInstructions));

            return SaveImage(imageModel);
        }

        private async Task<ImageModel> GetImage(string imageUrl)
        {
            var imageAsBytes = await GetImageFromSourceAsByteArray(imageUrl);

            var image = Image.Load(imageAsBytes, out var imageFormat);

            return new ImageModel(image, imageFormat);
        }

        private static ModifiedImage SaveImage(ImageModel imageModel)
        {
            using (var imageAsStream = new MemoryStream())
            {
                imageModel.Image.Save(imageAsStream, imageModel.ImageFormat);

                return new ModifiedImage(imageModel.ImageFormat.DefaultMimeType,
                    imageAsStream.ToArray());
            }
        }

        private async Task<byte[]> GetImageFromSourceAsByteArray(string imageUrl)
        {
            var bytes = await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync(imageUrl);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(
                        $"Could not get image from {imageUrl}, status code: {response.StatusCode}");

                return await response.Content.ReadAsByteArrayAsync();
            });

            return bytes;
        }
    }
}