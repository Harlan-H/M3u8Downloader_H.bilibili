using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Core.Streams;
using M3u8Downloader_H.bilibili.Core.User;
using M3u8Downloader_H.bilibili.Core.Videos;

namespace M3u8Downloader_H.bilibili.Core
{
    public class BiliCoreClient(HttpClient httpClient,ICacheService cacheService)
    {
        public UserClient Users { get; } = new UserClient(httpClient);

        public VideoClient Videos { get; } = new VideoClient(httpClient, cacheService);

        public StreamClient Streams { get; } = new StreamClient(httpClient, cacheService);
    }
}
