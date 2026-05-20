using M3u8Downloader_H.Abstractions.Common;
using M3u8Downloader_H.Abstractions.M3u8;
using M3u8Downloader_H.Abstractions.Plugins.Download;
using System.Diagnostics;
using System.Net.Http;

namespace M3u8Downloader_H.Gui.Utils
{
    internal class AppCommandService : IAppCommandService
    {
        public void DownloadByM3uFileInfo(HttpClient? httpClient, IDownloadParamBase downloadParamBase, IM3uFileInfo m3UFileInfo, IDownloadPlugin? downloadPlugin)
        {
            Debug.WriteLine(downloadParamBase);
        }

        public void DownloadByUrl(HttpClient? httpClient, IM3u8DownloadParam m3U8DownloadParam, IDownloadPlugin? downloadPlugin)
        {
            Debug.WriteLine(m3U8DownloadParam);
        }

        public void DownloadMedia(HttpClient? httpClient, IMediaDownloadParam mediaDownloadParam)
        {
            Debug.WriteLine(mediaDownloadParam);
        }
    }
}
