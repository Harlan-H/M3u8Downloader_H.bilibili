using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using M3u8Downloader_H.Abstractions.Models;
using M3u8Downloader_H.Abstractions.Plugins.Window;
using M3u8Downloader_H.bilibili;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace M3u8Downloader_H.Gui.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Main dllMain;

        [ObservableProperty]
        public partial UserControl  MainView { get; set; }


        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<IWindowContext>();
            dllMain = new Main();

            var windowPlugin = dllMain.CreateWindoPlugin();
            if (windowPlugin is null || windowPlugin is not IWindowPlugin windowInstance)
                throw new InvalidDataException("继承IWindowPlugin接口的类没有默认的构造函数");

            ServiceCollection services = new();
            services.AddSingleton(context.NotificationService);
            services.AddSingleton(context.AppCommandService);
            services.AddSingleton(context.PluginStorageService);
            services.AddSingleton(context.HttpFactory);
            windowInstance.ConfigureServices(services);

            var serviceProvider1 = services.BuildServiceProvider();

            var view = Activator.CreateInstance(windowInstance.MainWindowViewType);
            if (view is not UserControl control)
                throw new InvalidOperationException("ui接口继承有误 不是UserControl类型");

            var model = serviceProvider1.GetRequiredService(windowInstance.MainWindowViewModelType);
            control.DataContext = model;
            MainView = control;
        }
    }
}
