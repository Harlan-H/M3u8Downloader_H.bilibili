using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace M3u8Downloader_H.bilibili.Core.Models
{
    public partial class User : CommonResp<UserInfo>;

    public class UserInfo
    {
        [JsonPropertyName("isLogin")]
        public bool IsLogin { get; set; }

        [JsonPropertyName("uname")]
        public string  UserName { get; set; } = string.Empty;
    }

    [JsonSerializable(typeof(User))]
    public partial class UserContext : JsonSerializerContext;
}
