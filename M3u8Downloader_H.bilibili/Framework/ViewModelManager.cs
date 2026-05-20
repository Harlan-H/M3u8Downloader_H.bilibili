using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Services;
using M3u8Downloader_H.bilibili.ViewModels.Components;
using M3u8Downloader_H.bilibili.ViewModels.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace M3u8Downloader_H.bilibili.Framework
{
    public class ViewModelManager(IServiceProvider serviceProvider)
    {
        public LoginViewModel CreateLoginViewModel() => serviceProvider.GetRequiredService<LoginViewModel>();

        public UserService CreateUserService() => serviceProvider.GetRequiredService<UserService>();

        public DownloadSingleViewModel CreateDownloadSingleViewModel(Video video)
        {
            var singleViewModel = serviceProvider.GetRequiredService<DownloadSingleViewModel>();
            singleViewModel.Title = video.Title;
            singleViewModel.Owner = video.Owner.Author;
            singleViewModel.Description = video.Description;
            singleViewModel.Thumbnail = video.Thumbnail;
            singleViewModel.PlayList = video.PlayLists.Single();
            return singleViewModel;
        }

        public DownloadPageViewModel CreateDownloadPageViewModel(Video video)
        {
            var pageViewModel = serviceProvider.GetRequiredService<DownloadPageViewModel>();
            pageViewModel.Title = video.Title;
            pageViewModel.Owner = video.Owner.Author;
            pageViewModel.Description = video.Description;
            pageViewModel.Thumbnail = video.Thumbnail;
            return pageViewModel;
        }


    }
}
