using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Abstractions.Plugins.Window;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Services;
using M3u8Downloader_H.bilibili.ViewModels;
using M3u8Downloader_H.bilibili.ViewModels.Components;
using M3u8Downloader_H.bilibili.ViewModels.Dialogs;
using Microsoft.Extensions.DependencyInjection;


namespace M3u8Downloader_H.bilibili
{
    public class GUI : IWindowPlugin
    {
        public Type MainWindowViewType => typeof(MainWindowView);

        public Type MainWindowViewModelType => typeof(MainWindowViewModel);

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<BiliApiService>();
            services.AddSingleton<DownloadServices>();
            services.AddSingleton<SettingsService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ViewModelManager>();

            services.AddSingleton<MainWindowViewModel>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<DownloadPageViewModel>();
            services.AddTransient<DownloadSingleViewModel>();
            services.AddTransient<DownloadPageViewModel>();

        }
    }
}
