using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FFmpegDownloader.Dialogs;
using FFmpegDownloader.Enums;
using FFmpegDownloader.Models;
using FFmpegDownloader.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace FFmpegDownloader.ViewModels;

public sealed partial class DownloadsViewModel : ObservableObject
{
    private readonly IDownloadService downloadService;
    private readonly IContentDialogService dialogService;

    public ObservableCollection<DownloadItem> Downloads => downloadService.Downloads;

    public DownloadItem? CurrentDownloadingItem => downloadService.CurrentDownloadingItem;

    public ICollectionView PendingDownloads { get; }

    public ICollectionView CompletedDownloads { get; }

    public ICollectionView FailedDownloads { get; }

    public DownloadsViewModel(IDownloadService downloadService,
                              IContentDialogService dialogService)
    {
        this.downloadService = downloadService;
        this.dialogService = dialogService;

        CollectionViewSource pendingDownloadsSource = new() { Source = Downloads };
        CollectionViewSource completedDownloadsSource = new() { Source = Downloads };
        CollectionViewSource failedDownloadsSource = new() { Source = Downloads };

        PendingDownloads = pendingDownloadsSource.View;
        CompletedDownloads = completedDownloadsSource.View;
        FailedDownloads = failedDownloadsSource.View;

        PendingDownloads.Filter = item =>
            item is DownloadItem download && download.Status == DownloadStatus.Pending;

        CompletedDownloads.Filter = item =>
            item is DownloadItem download && download.Status == DownloadStatus.Completed;

        FailedDownloads.Filter = item =>
            item is DownloadItem download && download.Status == DownloadStatus.Failed;

        Downloads.CollectionChanged += OnDownloadsCollectionChanged;
    }

    private void OnDownloadsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (DownloadItem download in e.OldItems)
            {
                download.PropertyChanged -= OnDownloadPropertyChanged;
            }
        }

        if (e.NewItems is not null)
        {
            foreach (DownloadItem download in e.NewItems)
            {
                download.PropertyChanged += OnDownloadPropertyChanged;
            }
        }

        RefreshDownloadsViews();
    }

    private void OnDownloadPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DownloadItem.Status))
        {
            RefreshDownloadsViews();
        }
    }

    private void RefreshDownloadsViews()
    {
        PendingDownloads.Refresh();
        CompletedDownloads.Refresh();
        FailedDownloads.Refresh();
    }

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
}
