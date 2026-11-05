using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FFmpegDownloader.Dialogs;
using FFmpegDownloader.Enums;
using FFmpegDownloader.Models;
using FFmpegDownloader.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace FFmpegDownloader.ViewModels;

public sealed partial class DownloadsViewModel(IDownloadService downloadService,
                                               IContentDialogService dialogService) : ObservableObject
{
    public ObservableCollection<DownloadItem> Downloads => downloadService.Downloads;

    public bool IsQueueRunning => downloadService.IsQueueRunning;

    [RelayCommand]
    private async Task RemoveDownload(DownloadItem download)
    {
        RemoveDownloadConfirmationDialog dialog = new(download.FileName);
        ContentDialogResult result = await dialogService.ShowAsync(dialog, CancellationToken.None);

        if (result == ContentDialogResult.Primary)
        {
            downloadService.RemoveDownload(download);
        }
    }

    [RelayCommand]
    private async Task StartQueue()
    {
        await downloadService.StartQueue();
    }

    [RelayCommand]
    private async Task StopQueue()
    {
        bool hasActiveDownload = Downloads.Any(download => download.Status == DownloadStatus.Downloading);
        StopQueueConfirmationDialog dialog = new(hasActiveDownload);
        ContentDialogResult result = await dialogService.ShowAsync(dialog, CancellationToken.None);

        if (result == ContentDialogResult.Primary)
        {
            downloadService.StopQueue(dialog.StopCurrentDownloadCheckBox.IsChecked == true);
        }
    }

    [RelayCommand]
    private static void OpenOutputFolder(DownloadItem download)
    {
        string? directoryPath = Path.GetDirectoryName(download.OutputPath);

        if (Directory.Exists(directoryPath))
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = directoryPath,
                UseShellExecute = true
            });
        }
    }
}
