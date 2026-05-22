using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Abstractions.Plugins.Download;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Models;
using M3u8Downloader_H.bilibili.Services;
using System.Collections.ObjectModel;

namespace M3u8Downloader_H.bilibili.ViewModels.Components
{
    public partial class DownloadMultiViewModel : PluginViewModelBase
    {
        private readonly DownloadServices downloadService;
        private readonly ImageHelper imageHelper;
        private readonly INotificationService notificationService;

        private string SavePath 
        {
            get => field ??= Path.GetInvalidFileNameChars().Append('.').Aggregate(Title, (current, invalidChar) => current.Replace(invalidChar, '_'));
        }

        [ObservableProperty]
        public partial string Title { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Bitmap Thumbnail { get; set; } = default!;

        [ObservableProperty]
        public partial string Owner { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Description { get; set; } = string.Empty;

        public ObservableCollection<StreamViewModel> StreamViewModels { get; set; } = [];

        public ObservableCollection<StreamViewModel> SelectedStreamViewModels { get; } = [];

        public DownloadMultiViewModel(DownloadServices downloadService, ImageHelper imageHelper, INotificationService notificationService)
        {
            this.downloadService = downloadService;
            this.imageHelper = imageHelper;
            this.notificationService = notificationService;
            SelectedStreamViewModels.CollectionChanged += (_, _) =>
            {
                ConfirmCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            };
        }

        public async Task InitPageStreamDataAsync(Video video)
        {
            try
            {
                Thumbnail = await imageHelper.LoadFromUrlAsync(video.Thumbnail);

                foreach (var item in video.PlayLists)
                {
                    var streamViewModel = new StreamViewModel()
                    {
                        Page = item.Page,
                        Duration = item.Duration ?? TimeSpan.Zero,
                        Title = item.Title ?? "好像没有标题",
                        CTime = video.CTime
                    };
                    var streamdata = await downloadService.GetStreamDataAsync(video.Bvid, video.Aid, item);
                    streamViewModel.InitStreamDataAsync(streamdata);
                    StreamViewModels.Add(streamViewModel);
                    await Task.Delay(1);
                }
            }
            catch (Exception ex)
            {
                notificationService.Info($"获取音视频下载信息失败,{ex.Message}");
            }
        }

        public async Task InitEpisodeStreamDataAsync(UgcSeason ugcSeason)
        {
            try
            {
                Thumbnail = await imageHelper.LoadFromUrlAsync(ugcSeason.Pic);

                foreach (var (index, item) in ugcSeason.Sections[0].Episodes.Index())
                {
                    var streamViewModel = new StreamViewModel()
                    {
                        Page = index + 1,
                        Duration = item.PlayList.Duration ?? TimeSpan.Zero,
                        Title = item.Title,
                        CTime = item.Arc.Ctime
                    };
                    var streamdata = await downloadService.GetStreamDataAsync(item.Bvid, item.Aid, item.PlayList);
                    streamViewModel.InitStreamDataAsync(streamdata);
                    StreamViewModels.Add(streamViewModel);
                    await Task.Delay(1);
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
                    downloadService.DownloadMedia(SavePath, item.Title,item.SelectedVideoItem.Stream, item.AudioStreamInfo);
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
