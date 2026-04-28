using M3u8Downloader_H.Abstractions.Plugins;
using M3u8Downloader_H.Abstractions.Plugins.Download;
using M3u8Downloader_H.Abstractions.Plugins.Window;
using M3u8Downloader_H.Attributes.Attributes;

namespace M3u8Downloader_H.bilibili
{
    [Plugin("b站","主要支持b站的视频下载方案","Harlan","5.0.0",HasUi = true)]
    public class Main : IPluginEntry
    {
        public bool CanHandle(Uri url) => false;    

        public IDownloadPlugin? CreateDownloadPlugin() => null;

        public IWindowPlugin? CreateWindoPlugin() => new GUI();

    }
}
