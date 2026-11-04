using CommunityToolkit.Mvvm.ComponentModel;
using FFmpegDownloader.Enums;
using FFmpegDownloader.Models;
using System.Collections.ObjectModel;

namespace FFmpegDownloader.Services;

public sealed partial class DownloadService : ObservableObject, IDownloadService
{
    private CancellationTokenSource? currentDownloadCancellationTokenSource;
    private bool isQueueRunning;

    public ObservableCollection<DownloadItem> Downloads { get; } = [];

    [ObservableProperty]
    public partial DownloadItem? CurrentDownloadingItem { get; private set; }

    public void AddDownload(string url, string fileName, string outputPath)
    {
        Downloads.Add(new DownloadItem
        {
            Url = url,
            FileName = fileName,
            OutputPath = outputPath
        });
    }

    public void RemoveDownload(DownloadItem download)
    {
        // Un téléchargement en cours ne peut pas être supprimé directement.
        if (!ReferenceEquals(CurrentDownloadingItem, download))
        {
            Downloads.Remove(download);
        }
    }

    public async Task StartQueue()
    {
        if (isQueueRunning)
        {
            return;
        }

        isQueueRunning = true;

        foreach (DownloadItem download in Downloads)
        {
            if (download.Status != DownloadStatus.Pending)
            {
                continue;
            }

            download.Status = DownloadStatus.Downloading;
            CurrentDownloadingItem = download;

            currentDownloadCancellationTokenSource = new CancellationTokenSource();

            try
            {
                await SimulateDownloadAsync(download, currentDownloadCancellationTokenSource.Token);

                download.Status = DownloadStatus.Completed;
            }
            catch (OperationCanceledException)
                when (currentDownloadCancellationTokenSource.IsCancellationRequested)
            {
                // Le téléchargement a été interrompu volontairement.
                download.Status = DownloadStatus.Pending;
                download.Progress = 0;

                break;
            }
            catch (Exception)
            {
                download.Status = DownloadStatus.Failed;

                // Une erreur arrête la file, les suivants restent en attente.
                break;
            }
            finally
            {
                currentDownloadCancellationTokenSource.Dispose();
                currentDownloadCancellationTokenSource = null;
                CurrentDownloadingItem = null;
            }
        }

        isQueueRunning = false;
    }

    public void StopQueue(bool stopCurrentDownload)
    {
        if (isQueueRunning && stopCurrentDownload)
        {
            currentDownloadCancellationTokenSource?.Cancel();
        }

        isQueueRunning = false;
    }

    private const int SimulationDuration = 2000;
    private const int SimulationSteps = 20;
    private static async Task SimulateDownloadAsync(
        DownloadItem download,
        CancellationToken cancellationToken)
    {
        int delayPerStep = SimulationDuration / SimulationSteps;

        for (int step = 1; step <= SimulationSteps; step++)
        {
            await Task.Delay(delayPerStep, cancellationToken);
            download.Progress = step * 100.0 / SimulationSteps;
        }
    }
}
