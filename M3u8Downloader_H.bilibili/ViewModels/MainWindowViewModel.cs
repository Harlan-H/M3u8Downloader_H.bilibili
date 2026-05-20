using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Services;
using M3u8Downloader_H.bilibili.ViewModels.Dialogs;

namespace M3u8Downloader_H.bilibili.ViewModels
{
    public partial class MainWindowViewModel(
        INotificationService notificationService,
        ViewModelManager viewModelManager,
        SettingsService settingsService,
        DownloadServices downloadService) : PluginViewModelBase
    {
        private string oldRequestUrl = default!;

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
            if (RequestUrl.Equals(oldRequestUrl))
                return;

            oldRequestUrl = RequestUrl;

            try
            {
                var videoData = await downloadService.ParseQuery(RequestUrl);
                if(videoData.Video.VideoSize == 1)
                {
                    var downloadviewmodel = viewModelManager.CreateDownloadSingleViewModel(videoData.Video);
                    CurrentViewModel = downloadviewmodel;
                    _ = downloadviewmodel.InitStreamDataAsync(videoData.Video);
                }
                else if (videoData.Video.VideoSize > 1)
                {
                    var downloadPageViewModel  = viewModelManager.CreateDownloadPageViewModel(videoData.Video);
                    _ = downloadPageViewModel.InitStreamDataAsync(videoData.Video);
                    CurrentViewModel = downloadPageViewModel;
                }
                else
                {
                    CurrentViewModel = null;
                }
                
            }
            catch (Exception ex)
            {
                notificationService.Info(ex.Message);
            }
        }




    }
}
