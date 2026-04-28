using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Core.Streams;
using M3u8Downloader_H.bilibili.Core.Videos;

namespace M3u8Downloader_H.bilibili.Core
{
    public class BiliCoreClient(HttpClient httpClient)
    {
        public VideoClient Videos { get; } = new VideoClient(httpClient);

        public StreamClient Streams { get; } = new StreamClient(httpClient);
    }
}
