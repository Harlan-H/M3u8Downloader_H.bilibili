using M3u8Downloader_H.Abstractions.Common;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Gui.Utils;
using M3u8Downloader_H.Utils;
using System;

namespace M3u8Downloader_H.Gui.Models
{
    public class MyWindowContext: IWindowContext
    {
        public IHttpFactory HttpFactory => Http.Instance;

        public INotificationService NotificationService => new SnackbarManager(string.Empty,TimeSpan.Zero);
        public IAppCommandService AppCommandService => new AppCommandService();
        public IPluginStorage PluginStorageService => new PluginStorage("C:\\Users\\admin\\Desktop\\666\\PluginData");
    }
}
