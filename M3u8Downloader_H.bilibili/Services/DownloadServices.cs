using M3u8Downloader_H.bilibili.Core;
using M3u8Downloader_H.bilibili.Core.Extensions;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Core.Streams;
using M3u8Downloader_H.bilibili.Models;
using M3u8Downloader_H.Common.DownloadPrams;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Services
{
    internal class DownloadServices(BiliCoreClient biliCoreClient)
    {
        private Video Video = default!;
        public async Task<VideoData> ParseQuery(string? url)
        {
            url = url?.Trim();

            var videoId = VideoId.TryParse(url);
            if (videoId != null)
            {
                var videoData = await biliCoreClient.Videos.GetVideoInfoAsync(videoId.Value);
                Video = videoData.Video;
                return videoData;
            }

            throw new InvalidOperationException("不支持得请求地址");
        }

        public async Task<MediaDownloadParams> GetDownloadParam(PlayList playList)
        {
            StreamId streamId = new(Video.Bvid, Video.Aid, playList);
            var streamManifest = await biliCoreClient.Streams.GetStreamManifestAsync(streamId);
            streamManifest.EnsureSuccess();

            var video = streamManifest.Data.Dash.Videos.GetBestStreamInfoOptions();
            var audio = streamManifest.Data.Dash.Audios.GetBestStreamInfoOptions();

            Uri videoUri = new(video.BaserUrl);
            Uri audioUri = new(audio.BaserUrl);

            return new MediaDownloadParams(string.Empty, videoUri, audioUri, playList.Title, null)
            {
                IsVideoStream = true,
            };
        }
    }
}
