using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Gui.Models;
using M3u8Downloader_H.Gui.ViewModels;
using M3u8Downloader_H.Gui.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace M3u8Downloader_H.Gui
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider = default!;
        public App()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IWindowContext, MyWindowContext>();

            _serviceProvider = services.BuildServiceProvider();
        }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);


        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(_serviceProvider),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}