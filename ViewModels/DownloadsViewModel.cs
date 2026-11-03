using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FFmpegDownloader.Enums;
using FFmpegDownloader.Models;
using FFmpegDownloader.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace FFmpegDownloader.ViewModels;

public sealed partial class DownloadsViewModel : ObservableObject
{
    private readonly IDownloadService downloadService;
    private readonly IContentDialogService dialogService;

    public ObservableCollection<DownloadItem> Downloads => downloadService.Downloads;

    public int PendingCount => Downloads.Count(download => download.Status == DownloadStatus.Pending);

    public int DownloadingCount => Downloads.Count(download => download.Status == DownloadStatus.Downloading);

    public int CompletedCount => Downloads.Count(download => download.Status == DownloadStatus.Completed);

    public DownloadsViewModel(IDownloadService downloadService,
                              IContentDialogService dialogService)
    {
        this.downloadService = downloadService;
        this.dialogService = dialogService;

        Downloads.CollectionChanged += OnDownloadsCollectionChanged;

        foreach (DownloadItem download in Downloads)
        {
            download.PropertyChanged += OnDownloadPropertyChanged;
        }
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

        RefreshCounters();
    }

    private void OnDownloadPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DownloadItem.Status))
        {
            RefreshCounters();
        }
    }

    private void RefreshCounters()
    {
        OnPropertyChanged(nameof(PendingCount));
        OnPropertyChanged(nameof(DownloadingCount));
        OnPropertyChanged(nameof(CompletedCount));
    }

    private async Task<ContentDialogResult> GetRemoveConfirmationResult(DownloadItem download)
    {
        ContentDialog dialog = new()
        {
            Title = App.Localization["ConfirmDeletion"],
            Content = $"{App.Localization["Delete"]} « {download.FileName} » ?",
            PrimaryButtonText = App.Localization["Delete"],
            PrimaryButtonAppearance = ControlAppearance.Danger,
            CloseButtonText = App.Localization["Cancel"],
            DefaultButton = ContentDialogButton.Close
        };

        return await dialogService.ShowAsync(dialog, CancellationToken.None);
    }

    [RelayCommand]
    private async Task RemoveDownload(DownloadItem download)
    {
        ContentDialogResult result = await GetRemoveConfirmationResult(download);

        if (result == ContentDialogResult.Primary)
        {
            downloadService.RemoveDownload(download);
        }
    }

}
