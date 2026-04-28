using M3u8Downloader_H.bilibili.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Core.Extensions
{
    public static    class SteamManfestExtensinos
    {
        extension(StreamManifest streamManifest)
        {
            public void EnsureSuccess()
            {
                if (streamManifest.Code != 0)
                    throw new InvalidDataException($"获取数据失败:{streamManifest.Message}");
                
            }
        }
    }
}
