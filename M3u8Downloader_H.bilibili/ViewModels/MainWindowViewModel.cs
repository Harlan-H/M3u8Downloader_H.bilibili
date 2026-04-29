using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.bilibili.Core;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Core.Streams;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Services;
using M3u8Downloader_H.Common.DownloadPrams;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace M3u8Downloader_H.bilibili.ViewModels
{
    public partial class MainWindowViewModel : IPluginViewModelBase
    {
        private readonly IWindowContext windowContext;
        private readonly BiliCoreClient biliCoreClient;
        private readonly DownloadServices downloadService;
        private string oldRequestUrl = default!;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartParseCommand))]
        public partial string RequestUrl { get; set; } = default!;

        public ObservableCollection<PlayList> PlayLists { get; } = [];

        public ObservableCollection<PlayList> SelectedVideos { get; } = [];

        public MainWindowViewModel(IWindowContext windowContext) 
        {
            this.windowContext = windowContext;
            biliCoreClient = new BiliCoreClient(windowContext.ApiFactory.Client);
            downloadService = new DownloadServices(biliCoreClient);
            SelectedVideos.CollectionChanged += (_, _) =>
            {
                ConfirmCommand.NotifyCanExecuteChanged();
                CancelCommand.NotifyCanExecuteChanged();
            };
            
        }

        private bool CanStartParse => !string.IsNullOrWhiteSpace(RequestUrl);
        [RelayCommand(CanExecute = nameof(CanStartParse))]
        private async Task StartParse()
        {
            if (RequestUrl.Equals(oldRequestUrl))
                return;

            oldRequestUrl = RequestUrl;
            PlayLists.Clear();

            try
            {
                var videoData = await downloadService.ParseQuery(RequestUrl);
                foreach (var item in videoData.Video.PlayLists)
                {
                    PlayLists.Add(item);
                }
            }
            catch (Exception ex)
            {
                windowContext.SnackbarMaranger.Notify(ex.Message);
            }
        }


        private bool CanConfirm => SelectedVideos.Any();

        [RelayCommand(CanExecute = nameof(CanConfirm))]
        private async Task Confirm()
        {
            if (!SelectedVideos.Any())
                return;

            try
            {
                
                foreach (var item in SelectedVideos.ToList())
                {
                    var downloadParam = await downloadService.GetDownloadParam(item);
                    windowContext.AppCommandService.DownloadMedia(downloadParam);
                    await Task.Delay(20);
                    SelectedVideos.Remove(item);
                }

            }catch (Exception ex) {
                windowContext.SnackbarMaranger.Notify(ex.Message);
            }

        }

        [RelayCommand(CanExecute = nameof(CanConfirm))]
        private void Cancel()
        {
            SelectedVideos.Clear();
        }

    }
}
