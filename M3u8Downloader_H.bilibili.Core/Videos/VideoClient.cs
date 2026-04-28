using M3u8Downloader_H.bilibili.Core.Extensions;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace M3u8Downloader_H.bilibili.Core.Videos
{
    public partial class VideoClient(HttpClient httpClient)
    {
        public async ValueTask<VideoData> GetVideoInfoAsync(VideoId videoId, CancellationToken cancellationToken = default)
        {
            var raw = await httpClient.SendHttpRequestAsync(videoId.Url, cancellationToken);
            return GetVideo(raw);
        }

        public static VideoData GetVideo(string raw)
        {
            var InitState = InitialRegex().Match(raw).Groups[1].Value;
            if (string.IsNullOrWhiteSpace(InitState))
                throw new InvalidDataException("获取主页数据失败");

            return JsonSerializer.Deserialize(InitState, VideoContext.Default.VideoData)
                ?? throw new InvalidDataException("获取视频信息失败");
        }
        

        [GeneratedRegex(@"window\.__INITIAL_STATE__=([\s|\S]+);\(function", RegexOptions.Compiled)]
        private static partial Regex InitialRegex();
    }
}
