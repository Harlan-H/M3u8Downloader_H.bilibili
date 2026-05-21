using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Services;
using M3u8Downloader_H.bilibili.ViewModels.Dialogs;


namespace M3u8Downloader_H.bilibili.ViewModels
{
    public enum EpisodeMode { Single, Multi }

    public partial class MainWindowViewModel(
        INotificationService notificationService,
        ViewModelManager viewModelManager,
        SettingsService settingsService,
        DownloadServices downloadService) : PluginViewModelBase
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ShowLoginDialogCommand))]
        public partial string? UName { get; set; }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(StartParseCommand))]
        public partial string RequestUrl { get; set; } = default!;

        public bool ShowViewModel => CurrentViewModel is not null;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowViewModel))]
        public partial PluginViewModelBase? CurrentViewModel { get; set; } = default;

        [ObservableProperty]
        public partial EpisodeMode SelectedEpisode { get; set; } = EpisodeMode.Single;


        public override async Task InitializeAsync()
        {
            settingsService.Load();
            if (!string.IsNullOrEmpty(settingsService.Cookie))
            {
                try
                {
                    UserService userService = viewModelManager.CreateUserService();
                    UName = await userService.GetUserInfoAsync(settingsService.Cookie);
                }
                catch (Exception ex)
                {
                    notificationService.Info($"获取用户信息失败,{ex.Message}");
                }
            }
        }

        private bool CanShowLoginDialog => string.IsNullOrEmpty(UName);

        [RelayCommand(CanExecute = nameof(CanShowLoginDialog))]
        private async Task ShowLoginDialog()
        {
            LoginViewModel loginViewModel = viewModelManager.CreateLoginViewModel();
            var result = await DialogManager.ShowDialogAsync(loginViewModel);
            if(!string.IsNullOrEmpty(result))
            {
                UName = result;
            }
        }

        private bool CanStartParse => !string.IsNullOrWhiteSpace(RequestUrl);

        [RelayCommand(CanExecute = nameof(CanStartParse))]
        private async Task StartParse()
        {
            try
            {
                var videoData = await downloadService.ParseQuery(RequestUrl);
                if (videoData.Video.VideoSize == 1 && videoData.Video.UgcSeasons is null)
                {
                    var downloadviewmodel = viewModelManager.CreateDownloadSingleViewModel(videoData.Video);
                    _ = downloadviewmodel.InitStreamDataAsync(videoData.Video);
                    CurrentViewModel = downloadviewmodel;
                }
                //可能是合集 也可能是一个视频里多集 优先匹配多集
                else if (videoData.Video.VideoSize > 1)
                {
                    var downloadPageViewModel = viewModelManager.CreateDownloadPageViewModel(videoData.Video);
                    _ = downloadPageViewModel.InitPageStreamDataAsync(videoData.Video);
                    CurrentViewModel = downloadPageViewModel;
                }
                //一定是合集
                else if (videoData.Video.UgcSeasons is not null && videoData.Video.UgcSeasons.Sections[0].Episodes.Count > 1)
                {
                    if (SelectedEpisode == EpisodeMode.Single)
                    {
                        var downloadviewmodel = viewModelManager.CreateDownloadSingleViewModel(videoData.Video);
                        _ = downloadviewmodel.InitStreamDataAsync(videoData.Video);
                        CurrentViewModel = downloadviewmodel;
                    }
                    else if (SelectedEpisode == EpisodeMode.Multi)
                    {
                        var downloadviewModel = viewModelManager.CreateDownloadPageViewModel(videoData.Video);
                        _ = downloadviewModel.InitEpisodeStreamDataAsync(videoData.Video.UgcSeasons);
                        CurrentViewModel = downloadviewModel;
                    }
                }

            }
            catch (Exception ex)
            {
                notificationService.Info(ex.Message);
            }
        }




    }
}
