using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Abstractions.Plugins.Download;
using M3u8Downloader_H.bilibili.Core.Extensions;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Core.Streams;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Models;
using M3u8Downloader_H.bilibili.Services;
using System.Collections.ObjectModel;

namespace M3u8Downloader_H.bilibili.ViewModels.Components
{
    public partial class DownloadSingleViewModel(DownloadServices downloadServices,INotificationService notificationService) : PluginViewModelBase
    {
        private bool _isDownloaded = false;
        private List<StreamInfo> _audios = default!;

        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Uri Thumbnail { get; set; } = default!;

        [ObservableProperty]
        public partial string Owner { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Description { get; set; } = string.Empty;

        [ObservableProperty]
        public partial PlayList PlayList { get; set; }

        public ObservableCollection<StreamInfoItem> StreamInfoItems { get; set; } = [];

        [ObservableProperty]
        public partial StreamInfoItem SelectedVideoItem { get; set; }

        public async Task InitStreamDataAsync(Video video)
        {
            try
            {
                var streamdata = await downloadServices.GetStreamDataAsync(video, PlayList);
                _audios = [.. streamdata.Dash.Audios.GetBestStreamInfos()];
                DownloadServices.PopulateStreamInfos(StreamInfoItems, streamdata.Dash.Videos, streamdata.SupportFormats);
                SelectedVideoItem = StreamInfoItems.First();
            }
            catch (Exception ex)
            {
                notificationService.Info($"获取音视频下载信息失败,{ex.Message}");
            }
        }

        private bool CanConfirm => _isDownloaded is false && PlayList.Cid != 0;
        [RelayCommand(CanExecute = nameof(CanConfirm))]
        private async Task Confirm()
        {
            if (_isDownloaded)
                return;

            try
            {
                downloadServices.DownloadMedia(string.Empty, Title, SelectedVideoItem.Stream, _audios.First());

                _isDownloaded = true;
                notificationService.Info($"已经开始下载,请点击左边基础查看");
            }
            catch (Exception ex)
            {
                notificationService.Info(ex.Message);
            }

        }
    }
}
