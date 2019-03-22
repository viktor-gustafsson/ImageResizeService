namespace ImageResizeService.Services.ImageProcessor.Models
{
    public class ModifiedImage
    {
        public ModifiedImage(string imageFormat, byte[] imageAsBytes)
        {
            ImageFormat = imageFormat;
            ImageAsBytes = imageAsBytes;
        }

        public string ImageFormat { get; }
        public byte[] ImageAsBytes { get; }
    }
}