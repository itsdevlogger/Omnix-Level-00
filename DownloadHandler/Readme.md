# Image Downloader Module
This module provides functionality to download images from URLs and set them as sprites for Unity game objects. It includes methods to download single images or multiple images asynchronously.

# Installation
Simply import the unity package to your project

# Usage
## ImageDownloader Class
### Downloading a Single Image
```csharp
ImageDownloader.Download(url, callback);
```
Where:
- `url`: The URL of the image to download.
- `callback`: A callback function to be invoked when the download is completed. The downloaded image will be passed as a `Sprite` parameter.

If you need access to some data, inside the callback, you can use:
```csharp
ImageDownloader.Download(url, userData, callback);
```
Where:
- `url`: The URL of the image to download.
- `userData`: Any additional data you want to pass along with the download request.
- `callback`: A callback function to be invoked when the download is completed. The downloaded image will be passed as a `Sprite` parameter along with the `userData`.

### Downloading Multiple Images
```csharp
ImageDownloader.Download(urlsWithIndividualCallbacks, onAllDownloaded);
```
- `urlsWithIndividualCallbacks`: An enumerable of tuples, where each tuple contains the URL of an image and a callback function to be invoked when that image is downloaded.
- `onAllDownloaded`: A callback function to be invoked when all images are downloaded.

If you need access to some data, inside the callback, you can use:
```csharp
ImageDownloader.Download(urlsWithIndividualCallbacks, onAllDownloaded);
```
- `urlsWithIndividualCallbacks`: An enumerable of tuples, where each tuple contains the URL of an image, user data, and a callback function to be invoked when that image is downloaded.
- `onAllDownloaded`: A callback function to be invoked when all images are downloaded.


## Extensions Class
This class provides extension methods for Unity UI components to simplify image downloading and setting.

### Downloading and Setting Images
```csharp
image.DownloadAndSet(url, callback);
```

- `image`: The Unity UI component (Image, SpriteRenderer, or RawImage) to set the downloaded image to.
- `url`: The URL of the image to download.
- `callback`: (Optional) A callback function to be invoked when the download is completed. The downloaded image will be passed as a `Sprite` parameter.

If you need access to some data, inside the callback, you can use:
```csharp
image.DownloadAndSet(url, userData, callback);
```

- `image`: The Unity UI component (Image, SpriteRenderer, or RawImage) to set the downloaded image to.
- `url`: The URL of the image to download.
- `userData`: Any additional data you want to pass along with the download request.
- `callback`: (Optional) A callback function to be invoked when the download is completed. The downloaded image will be passed as a `Sprite` parameter along with the `userData`.

## Notes
- Make sure your Unity project has internet access for downloading images from URLs.
