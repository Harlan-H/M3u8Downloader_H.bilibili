using Avalonia.Controls;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Abstractions.Plugins.Window;
using M3u8Downloader_H.bilibili.ViewModels;


namespace M3u8Downloader_H.bilibili
{
    public class GUI : IWindowPlugin
    {
        private IWindowContext _windowContext = default!;
        public void InitializeWindow(IWindowContext windowContext)
        {
            _windowContext = windowContext;
        }

        public UserControl CreateMainView()
        {
            return new MainWindowView()
            {
                DataContext = new MainWindowViewModel(_windowContext)
            };
        }
    }
}
