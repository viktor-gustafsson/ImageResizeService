# Imagecropperandresizer

Imagerecropperandresizer is a .Net Core web api that handles image cropping and resizing.

The application targets .Net Core 2.2

##
Master branch is making use of the SixLabors.ImageSharp nuget package for image mutation.
##
Drawing branch is making use of the System.Drawing implementation port to .Net Core for image mutation
##
SkiaSharp branch is making use of the SkiaSharp nuget package for image mutation 

There is a Polly implementation as well to handle retrying to get images from the source.

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
x(int) - x possition to crop from
y(int) - y possition to crop from
widthtocrop(int) - width to crop from crop possition
heighttocrop(int) - height to crop from crop possition
```
