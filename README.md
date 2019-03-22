# Imagecropperandresizer

Imagerecropperandresizer is a .Net Core web api that handles image cropping and resizing.

The application targets .Net Core 2.1
The primary implementation is SkiaSharp and it unfortunatley needs to target .net core 2.1 to be able to run inside a docker container.

There is a Polly implementation to handle retry logic when getting images from the source.

### Docker
It is very easy to install and deploy in a Docker container.

```sh
cd imagecropperandresizer
docker build -t ImageResizeService -f ImageResizeService/Dockerfile .
```

Once done, run the Docker image and map the port to whatever you wish on your host. In this example, we simply map port 5000 of the host to port 80 of the Docker (or whatever port was exposed in the Dockerfile):

```sh
docker run -p 5000:80 -e ASPNETCORE_URLS=http://+:80 ImageResizeService
```

# Other branches

## ImageSharp
ImageSharp branch is making use of the SixLabors.ImageSharp nuget package for image mutation.
## Drawing
Drawing branch is making use of the System.Drawing implementation port to .net Core for image mutation


# Endpoints
All height and width values must be possitive integers larger than 0.

### api/image/resize
```
GET
query parameters: 
url(string) - url to image
width(int) - new width
height(int) - new height
```
### api/image/crop
Image cropping is only done in the shape of a square.
```
GET
query parameters: 
url(string) - url to image
width(int) - width of final image
height(int) - height of final image
top(int) - top edge of crop box
left(int) - left edge of crop box
right(int) - right edge of crop box
bottom(int) - bottom edge of crop box
```
