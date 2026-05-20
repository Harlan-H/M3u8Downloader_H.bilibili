using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Services;
using System.Collections.ObjectModel;

namespace M3u8Downloader_H.bilibili.ViewModels.Components
{
    public partial class DownloadPageViewModel : PluginViewModelBase
    {
        private readonly DownloadServices downloadService;
        private readonly INotificationService notificationService;

        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Uri Thumbnail { get; set; } = default!;

        [ObservableProperty]
        public partial string Owner { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Description { get; set; } = string.Empty;

        public ObservableCollection<StreamViewModel> StreamViewModels { get; set; } = [];

        public ObservableCollection<StreamViewModel> SelectedStreamViewModels { get; } = [];

        public DownloadPageViewModel(DownloadServices downloadService,INotificationService notificationService)
        {
            this.downloadService = downloadService;
            this.notificationService = notificationService;
            SelectedStreamViewModels.CollectionChanged += (_, _) =>
            {
                ConfirmCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            };
        }

        public async Task InitStreamDataAsync(Video video)
        {
            try
            {
                foreach (var item in video.PlayLists)
                {
                    var streamViewModel = new StreamViewModel(item);
                    var streamdata = await downloadService.GetStreamDataAsync(video, item);
                    streamViewModel.InitStreamDataAsync(streamdata);
                    StreamViewModels.Add(streamViewModel);
                    await Task.Delay(20);
                }
            }
            catch (Exception ex)
            {
                notificationService.Info($"获取音视频下载信息失败,{ex.Message}");
            }
        }



        private bool CanConfirm => SelectedStreamViewModels.Any();

        [RelayCommand(CanExecute = nameof(CanConfirm))]
        private async Task Confirm()
        {
            if (!SelectedStreamViewModels.Any())
                return;

            try
            {

                foreach (var item in SelectedStreamViewModels.ToList())
                {
                    downloadService.DownloadMedia(Title,item.Title,item.SelectedVideoItem.Stream, item.AudioStreamInfo);
                    await Task.Delay(20);
                    SelectedStreamViewModels.Remove(item);
                }
                notificationService.Info($"已经开始下载,请点击左边基础查看");
            }
            catch (Exception ex)
            {
                notificationService.Info(ex.Message);
            }

        }

        [RelayCommand(CanExecute = nameof(CanConfirm))]
        private void Cancel()
        {
            SelectedStreamViewModels.Clear();
        }

    }
}
