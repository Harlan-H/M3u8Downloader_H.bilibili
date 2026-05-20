using M3u8Downloader_H.bilibili.Core.Extensions;
using M3u8Downloader_H.bilibili.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace M3u8Downloader_H.bilibili.Core.Streams
{
    public class StreamClient(HttpClient httpClient)
    {
        public async ValueTask<StreamData> GetStreamManifestAsync(
           StreamId streamId,
           CancellationToken cancellationToken = default)
        {
            var raw = await httpClient.SendHttpRequestAsync(streamId.PlayUrl, cancellationToken);
            return GetStreamData(raw);
        }

        public static StreamData GetStreamData(string raw)
        {
            var streamManifest = JsonSerializer.Deserialize(raw, StreamContext.Default.StreamManifest)
                   ?? throw new InvalidDataException("获取视频流出错");

            if (streamManifest.Code != 0)
                throw new InvalidDataException(streamManifest.Message);
            return streamManifest.Data;
        }
    }
}
