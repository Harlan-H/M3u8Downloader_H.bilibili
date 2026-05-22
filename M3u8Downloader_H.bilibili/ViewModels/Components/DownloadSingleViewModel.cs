using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.bilibili.Core.Extensions;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Models;
using M3u8Downloader_H.bilibili.Services;
using System.Collections.ObjectModel;

namespace M3u8Downloader_H.bilibili.ViewModels.Components
{
    public partial class DownloadSingleViewModel(DownloadServices downloadServices, ImageHelper imageHelper, INotificationService notificationService) : PluginViewModelBase
    {
        private List<StreamInfo> _audios = default!;

        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Bitmap? Thumbnail { get; set; } = default!;

        [ObservableProperty]
        public partial string Owner { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTime CTime { get; set; } = default!;

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
               
                Thumbnail = await imageHelper.LoadFromUrlAsync(video.Thumbnail);

                var streamdata = await downloadServices.GetStreamDataAsync(video.Bvid,video.Aid, PlayList);
                _audios = [.. streamdata.Dash.Audios.GetBestStreamInfos()];
                DownloadServices.PopulateStreamInfos(StreamInfoItems, streamdata.Dash.Videos, streamdata.SupportFormats);
                SelectedVideoItem = StreamInfoItems.First();
            }
            catch (Exception ex)
            {
                notificationService.Info($"获取音视频下载信息失败,{ex.Message}");
            }
        }


        private bool CanConfirm => PlayList.Cid != 0;
        [RelayCommand(CanExecute = nameof(CanConfirm))]
        private async Task Confirm()
        {

            try
            {
                downloadServices.DownloadMedia(string.Empty, Title, SelectedVideoItem.Stream, _audios.First());

                notificationService.Info($"已经开始下载,请点击左边基础查看");
            }
            catch (Exception ex)
            {
                notificationService.Info(ex.Message);
            }

        }
    }
}
