using Avalonia.Media;
using Avalonia.Media.Imaging;
using M3u8Downloader_H.Abstractions.Models;
using Microsoft.Extensions.Caching.Memory;


namespace M3u8Downloader_H.bilibili.Services
{
    public class ImageHelper(BiliApiService biliApiService,ICacheService cacheService)
    {
        private readonly Dictionary<string, WeakReference<Bitmap>> _bitmapCache = [];

        public async Task<Bitmap> LoadFromUrlAsync(Uri url)
        {
            if (_bitmapCache.TryGetValue(url.OriginalString, out var weak))
            {
                if (weak.TryGetTarget(out var bmp))
                {
                    return bmp;
                }
            }

            var result = await cacheService.GetOrCreateAsync(url.OriginalString, async entry =>
            {
                entry.Priority = CacheItemPriority.Low;

                using var response = await biliApiService.Client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                var contentType =
                    response.Content.Headers.ContentType?.MediaType;

                if (contentType is null ||
                    !contentType.StartsWith("image/"))
                {
                    throw new InvalidOperationException(
                        "Response is not image");
                }

                var bytes = await response.Content.ReadAsByteArrayAsync();

                entry.Size = bytes.Length;

                entry.SlidingExpiration =
                    TimeSpan.FromMinutes(5);

                return bytes;
            }) ?? throw new InvalidDataException("Image load failed");

            var bitmap = new Bitmap(new MemoryStream(result));
            _bitmapCache[url.OriginalString] = new WeakReference<Bitmap>(bitmap);
            return bitmap;
        }
    }
}

