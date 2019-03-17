# Imagecropperandresizer

Imagerecropperandresizer is a .Net Core web api that handles image cropping and resizing.

The application targets .Net Core 2.2 and it is making use of the SixLabors.ImageSharp nuget package for image mutation.
There is an alternet version avilable in the drawing branch using System.Drawing instead.

There is a Polly implementation as well to handle things like retrying to get images from the source.

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
