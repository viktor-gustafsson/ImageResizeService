# ImageResizeService

ImageResizeService is a .Net Core web api that handles image cropping and resizing.

The application targets .Net Core 2.1
The primary implementation is SkiaSharp and it unfortunatley needs to target .net core 2.1 to be able to run inside a docker container.

There is a Polly implementation to handle retry logic when getting images from the source.

### Docker
It is very easy to install and deploy in a Docker container.

```sh
docker build -t imageresizeservice -f ImageResizeService/Dockerfile .
```

Once done, run the Docker image and map the port to whatever you wish on your host. In this example, we simply map port 5000 of the host to port 80 of the Docker (or whatever port was exposed in the Dockerfile):

```sh
docker run -p 5000:80 -e ASPNETCORE_URLS=http://+:80 imageresizeservice
```

# Usage

## Shared query parameters
```
These parameters are shared between both resize and crop endpoint.
url(string) - url to image, needs to be url encoded.
width(int) - new width, needs to be 0 or larger.
height(int) - new height, needs to be 0 or larger.
imageFormat(string) - format of the output image, supported types jpeg and png.
jpegQuality(int) - OPTIONAL, needs to be between 0 and 100, only applies to jpeg images, if no value is supplied it will default to 100.
```

### api/image/resize
```
GET
endpoint specific query parameters:
```

### api/image/crop
Image cropping is only done in the shape of a square.
```
GET
endpoint specific query parameters: 
top(int) - top edge of crop box, needs to be 0 or larger.
left(int) - left edge of crop box, needs to be 0 or larger.
right(int) - right edge of crop box, needs to be 0 or larger.
bottom(int) - bottom edge of crop box, needs to be 0 or larger.
```

# Other branches

## ImageSharp
ImageSharp branch is making use of the SixLabors.ImageSharp nuget package for image mutation.
## Drawing
Drawing branch is making use of the System.Drawing implementation port to .net Core for image mutation
