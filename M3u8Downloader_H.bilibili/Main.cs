using M3u8Downloader_H.Abstractions.Plugins;
using M3u8Downloader_H.Abstractions.Plugins.Download;
using M3u8Downloader_H.Abstractions.Plugins.Window;
using M3u8Downloader_H.Attributes.Attributes;

namespace M3u8Downloader_H.bilibili
{
    [Plugin("bilibili", "b站的bv系列视频下载","Harlan","5.1.1",HasUi = true)]
    public class Main : IPluginEntry
    {
        public bool CanHandle(Uri url) => false;    

        public IDownloadPlugin? CreateDownloadPlugin() => null;

        public IWindowPlugin? CreateWindoPlugin() => new GUI();

    }
}
