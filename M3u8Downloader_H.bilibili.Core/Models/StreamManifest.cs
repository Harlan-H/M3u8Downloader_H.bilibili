using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace M3u8Downloader_H.bilibili.Core.Models
{
    public class StreamManifest : CommonResp<StreamData>;

    public class StreamData
    {
        [JsonPropertyName("dash")]
        public StreamDash Dash { get; set; } = default!;

        [JsonPropertyName("support_formats")]
        public List<SupportFormat> SupportFormats { get; set; } = default!;
    }


    public class StreamDash
    {
        [JsonPropertyName("video")]
        public List<StreamInfo> Videos { get; set; } = default!;

        [JsonPropertyName("audio")]
        public List<StreamInfo> Audios { get; set; } = default!;
    }

    public class StreamInfo
    {
        [JsonPropertyName("id")]
        public int Quality { get; set; }

        [JsonPropertyName("backupUrl")]
        public List<string> BaseUrls { get; set; } = default!;

        [JsonPropertyName("bandwidth")]
        public int BandWidth { get; set; }

        [JsonPropertyName("codecs")]
        public string Codec { get; set; } = default!;

    }

    public class SupportFormat
    {
        [JsonPropertyName("quality")]
        public int Quality { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; } = default!;

        [JsonPropertyName("new_description")]
        public string Description { get; set; } = default!;
    }

    [JsonSerializable(typeof(StreamManifest))]
    public partial class StreamContext : JsonSerializerContext;
}
