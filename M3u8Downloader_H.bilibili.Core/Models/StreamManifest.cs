using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace M3u8Downloader_H.bilibili.Core.Models
{
    public class StreamManifest
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = default!;

        [JsonPropertyName("data")]
        public StreamData Data { get; set; } = default!;
    }

    public class StreamData
    {
        [JsonPropertyName("dash")]
        public StreamDash Dash { get; set; } = default!;
    }

    public class StreamDash
    {
        [JsonPropertyName("video")]
        public IList<StreamInfo> Videos { get; set; } = default!;

        [JsonPropertyName("audio")]
        public IList<StreamInfo> Audios { get; set; } = default!;
    }

    public class StreamInfo
    {
        [JsonPropertyName("id")]
        public int Quality { get; set; }
        [JsonPropertyName("baseUrl")]
        public string BaserUrl { get; set; } = default!;

        [JsonPropertyName("bandwidth")]
        public int BandWidth { get; set; }

        [JsonPropertyName("codecs")]
        public string Codec { get; set; } = default!;

    }

    [JsonSerializable(typeof(StreamManifest))]
    public partial class StreamContext : JsonSerializerContext;
}
