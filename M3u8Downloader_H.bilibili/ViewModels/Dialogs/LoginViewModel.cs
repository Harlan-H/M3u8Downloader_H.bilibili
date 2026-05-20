using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Services;

namespace M3u8Downloader_H.bilibili.ViewModels.Dialogs
{
    public partial class LoginViewModel(BiliApiService biliApiService,SettingsService settingsService) : DialogViewModelBase<string>
    {
        [ObservableProperty]
        public partial string Cookie { get; set; }

        [ObservableProperty]
        public partial string ErrorString {  get; set; }

        [RelayCommand]
        private async Task Login(string cookie)
        {
            try
            {
                UserService userService = new(biliApiService);
                var resp = await userService.GetUserInfoAsync(cookie);
                base.Close(resp);
                settingsService.Cookie = Cookie;
                settingsService.Save();
            }
            catch (Exception ex) {
                ErrorString = ex.Message;
            }
        }

        [RelayCommand]
        private void Close()
        {
            base.Close();
        }
    }
}
