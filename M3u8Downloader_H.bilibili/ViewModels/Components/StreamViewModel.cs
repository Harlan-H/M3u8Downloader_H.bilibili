using CommunityToolkit.Mvvm.ComponentModel;
using M3u8Downloader_H.Abstractions.Plugins.Download;
using M3u8Downloader_H.bilibili.Core.Extensions;
using M3u8Downloader_H.bilibili.Core.Models;
using M3u8Downloader_H.bilibili.Framework;
using M3u8Downloader_H.bilibili.Models;
using M3u8Downloader_H.bilibili.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace M3u8Downloader_H.bilibili.ViewModels.Components
{
    public partial class StreamViewModel(PlayList playList) : PluginViewModelBase
    {

        [ObservableProperty]
        public partial int Page { get; set; } = playList.Page;

        [ObservableProperty]
        public partial string Title { get; set; } = playList.Title ?? "好像没有标题";

        [ObservableProperty]
        public partial TimeSpan Duration { get; set; } = playList.Duration ?? TimeSpan.Zero;

        [ObservableProperty]
        public partial DateTime CTime { get; set; } = playList.CTime;

        public ObservableCollection<StreamInfoItem> StreamInfoItems { get; set; } = [];

        [ObservableProperty]
        public partial StreamInfoItem SelectedVideoItem { get; set; }

        public StreamInfo AudioStreamInfo { get; private set; } = default!;


        public void InitStreamDataAsync(StreamData streamData)
        {
            AudioStreamInfo = streamData.Dash.Audios.GetBestStreamInfoOptions();
            DownloadServices.PopulateStreamInfos(StreamInfoItems, streamData.Dash.Videos, streamData.SupportFormats);
            SelectedVideoItem = StreamInfoItems.First();
        }
    }

}
