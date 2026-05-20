using Avalonia.Controls;
using M3u8Downloader_H.bilibili.Framework;

namespace M3u8Downloader_H.bilibili;

public partial class MainWindowView : UserControl
{
    public MainWindowView()
    {
        InitializeComponent();
    }

    private async void UserControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is MainWindowView view && view.DataContext is PluginViewModelBase viewmodel)
           await viewmodel.InitializeAsync();
    }
}