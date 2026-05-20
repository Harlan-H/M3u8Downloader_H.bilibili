using M3u8Downloader_H.bilibili.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Core.Extensions
{
    public static class StreamInfoExtensions
    {
        extension(IList<StreamInfo> streamInfo)
        {
            public StreamInfo GetBestStreamInfoOptions()
            {
                return streamInfo.GetBestStreamInfos()
                            .FirstOrDefault()
                            ?? throw new InvalidDataException("没有匹配到任何流数据");
            }

            public IEnumerable<StreamInfo> GetBestStreamInfos()
            {
                return streamInfo
                            .OrderByDescending(o => o.Quality)
                            .ThenByDescending(o => o.BandWidth);
            }

        }
    }
}
