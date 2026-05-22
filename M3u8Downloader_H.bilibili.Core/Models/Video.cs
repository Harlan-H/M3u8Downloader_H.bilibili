using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace M3u8Downloader_H.bilibili.Core.Models
{
    public class VideoData
    {
        [JsonPropertyName("videoData")]
        public Video Video { get; set; } = default!;
    }

    public class Video
    {
        public string Bvid { get; set; } = string.Empty;

        public long Aid { get; set; }

        [JsonPropertyName("videos")]
        public int VideoSize { get; set; }

        public string Title { get; set; } = default!;

        [JsonPropertyName("ctime")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime CTime { get; set; }

        [JsonPropertyName("desc")]
        public string Description { get; set; } = default!;

 
        public Owner Owner { get; set; } = default!;

        [JsonPropertyName("pic")]
        public Uri Thumbnail { get; set; } = default!;

        [JsonPropertyName("pages")]
        public List<PlayList> PlayLists { get; set; } = default!;

        [JsonPropertyName("ugc_season")]
        public UgcSeason UgcSeasons { get; set; } = default!;
    }

    public class Owner
    {
        [JsonPropertyName("name")]
        public string Author { get; set; } = default!;
    }

    public class PlayList
    {
        [JsonPropertyName("cid")]
        public long Cid { get; set; }

        [JsonPropertyName("page")]
        public int Page {  get; set; }

        [JsonPropertyName("part")]
        public string? Title { get; set; }

        [JsonPropertyName("duration")]
        [JsonConverter(typeof(TimeSpanJsonConverter))]
        public TimeSpan? Duration { get; set; }
    }



    public class UgcSeason {

        [JsonPropertyName("title")]
        public string Title { get; set; } = default!;

        [JsonPropertyName("cover")]
        public Uri Pic { get; set; } = default!;

        [JsonPropertyName("intro")]
        public string Description { get; set; } = default!;

        public List<UgcSection> Sections { get; set; } = default!;
    }

    public class UgcSection
    {
        public List<VideoEpisode> Episodes { get; set; } = default!;
    }

    public class VideoEpisode
    {
        public long Aid { get; set; }

        public string Title { get; set; } = default!;

        [JsonPropertyName("arc")]
        public Arc Arc { get; set; } = default!;

        [JsonPropertyName("page")]
        public PlayList PlayList { get; set; } = default!;

        public string Bvid { get; set; } = default!;
    }

    public class Arc
    {
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime Ctime { get; set; } = default!;
    }

    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    [JsonSerializable(typeof(VideoData))]
    public partial class VideoContext : JsonSerializerContext;


    public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetDouble(out double sec))
                    return TimeSpan.FromSeconds(sec);
            }
            return TimeSpan.Zero;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.TotalSeconds);
        }
    }

    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt64(out long sec))
                    return DateTimeOffset.FromUnixTimeSeconds(sec).LocalDateTime;
            }
            return new DateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.Second);
        }
    }

}
