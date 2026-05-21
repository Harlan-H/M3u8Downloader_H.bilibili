using M3u8Downloader_H.Abstractions.Common;
using M3u8Downloader_H.bilibili.Core.Extensions;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Core.Streams;
using M3u8Downloader_H.bilibili.Models;
using M3u8Downloader_H.Common.DownloadPrams;


namespace M3u8Downloader_H.bilibili.Services
{
    
    public class DownloadServices(BiliApiService biliApiService,IAppCommandService appCommandService)
    {
        public HttpClient HttpClient => biliApiService.Client;

        public async Task<VideoData> ParseQuery(string? url)
        {
            url = url?.Trim();
            var videoId = VideoId.TryParse(url);
            if (videoId != null)
            {
                var videoData = await biliApiService.BiliClient.Videos.GetVideoInfoAsync(videoId.Value);
                return videoData;
            }

            throw new InvalidOperationException("不支持得请求地址");
        }

        public static void PopulateStreamInfos(IList<StreamInfoItem> streamInfoItems, IList<Core.Models.StreamInfo> streamInfos, List<SupportFormat> supportFormats)
        {
            var bestStreamInfo = streamInfos.GetBestStreamInfos();
            foreach (var streamInfo in bestStreamInfo)
            {
                var supportFormat = supportFormats.First(supportFormat => streamInfo.Quality == supportFormat.Quality);
                streamInfoItems.Add(new StreamInfoItem(streamInfo, supportFormat));
            }

        }

        public async Task<StreamData> GetStreamDataAsync(string bvid,long aid, PlayList playList)
        {
            StreamId streamId = new(bvid, aid, playList);
            return  await biliApiService.BiliClient.Streams.GetStreamManifestAsync(streamId);
        }

        public void DownloadMedia(string savePath , string title, Core.Models.StreamInfo video, Core.Models.StreamInfo audio)
        {
            int index = Random.Shared.Next(video.BaseUrls.Count);
            Uri videoUri = new(video.BaseUrls[index]);
            Uri audioUri = new(audio.BaseUrls[index]);

            var param = new MediaDownloadParams(savePath, videoUri, audioUri, title, null)
            {
                IsVideoStream = true,
            };
            appCommandService.DownloadMedia(biliApiService.Client, param);
        }
    }
}
