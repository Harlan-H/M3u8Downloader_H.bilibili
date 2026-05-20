using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Common.Services;
using System.Text.Json.Serialization;

namespace M3u8Downloader_H.bilibili.Services
{
    public partial class SettingsService(IPluginStorage pluginStorage) : SettingsBase(SerializerContext.Default, pluginStorage.GetPath("Settings.dat"))
    {
        public string Cookie { get; set; } = default!;
    }

    public partial class SettingsService
    {
        [JsonSerializable(typeof(SettingsService))]
        private partial class SerializerContext : JsonSerializerContext;
    }
}
