using Avalonia.Controls;
using Avalonia.Controls.Templates;
using M3u8Downloader_H.bilibili.ViewModels.Components;
using M3u8Downloader_H.bilibili.ViewModels.Dialogs;

namespace M3u8Downloader_H.bilibili.Framework
{
    public class ViewLocator : IDataTemplate
    {
        private Control? TryCreateView(PluginViewModelBase viewModel) =>
               viewModel switch
               {
                   LoginViewModel => new LoginView(),
                   DownloadSingleViewModel => new DownloadSingleView(),
                   DownloadPageViewModel => new DownloadPageView(),
                   DownloadEpisodeViewModel => new DownloadEpiscodeView(),
                   _ => null,
               };

        public Control? TryBindView(PluginViewModelBase viewModel)
        {
            var view = TryCreateView(viewModel);
            if (view is null)
                return null;

            view.DataContext ??= viewModel;
            return view;
        }

        public Control? Build(object? param)
        {
            return param is PluginViewModelBase viewModel ? TryBindView(viewModel) : null;
        }

        public bool Match(object? data)
        {
            return data is PluginViewModelBase;
        }
    }
}
