# Imagerecropperandresizer

Imagerecropperandresizer is a .Net Core web api that handles image cropping and resizing.

The application targets .Net Core 2.2 and it is making use of the SixLabors.ImageSharp nuget package for image mutation.
There is a Polly implementation as well to handle things like retrying to get images from the source.
Newtonsoft Json is used to deserialize responses from the http client.


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
width(int) - width to crop from crop possition
height(int) - height to crop from crop possition
x(int) - x possition to crop from
y(int) - y possition to crop from
```
