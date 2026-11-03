using FFmpegDownloader.Services;
using FFmpegDownloader.ViewModels;
using FFmpegDownloader.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace FFmpegDownloader;

public partial class App : Application
{
    private readonly IHost _host;

    public static readonly Localization Localization = new();

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Navigation
                services.AddNavigationViewPageProvider();

                // ViewModels
                services.AddSingleton<MainViewModel>()
                        .AddTransient<DownloadsViewModel>()
                        .AddTransient<AddDownloadViewModel>()
                        .AddTransient<SettingsViewModel>();

                // Pages
                services.AddTransient<DownloadsView>()
                        .AddTransient<AddDownloadView>()
                        .AddTransient<SettingsView>();

                // Services
                services.AddSingleton<IDownloadService, DownloadService>()
                        .AddSingleton<IContentDialogService, ContentDialogService>();

                // Fenêtre principale
                services.AddSingleton<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }
}
