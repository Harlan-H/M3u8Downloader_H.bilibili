using M3u8Downloader_H.bilibili.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Models
{
    public record StreamInfoItem(StreamInfo Stream, SupportFormat Format)
    {
        public string Display => $"{Format.Description} {Stream.Codec}";
    }
}
