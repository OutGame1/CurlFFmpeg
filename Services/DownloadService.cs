using CommunityToolkit.Mvvm.ComponentModel;
using FFmpegDownloader.Enums;
using FFmpegDownloader.Models;
using System.Collections.ObjectModel;

namespace FFmpegDownloader.Services;

public sealed partial class DownloadService : ObservableObject, IDownloadService
{
    private CancellationTokenSource? currentDownloadCancellationTokenSource;
    public bool IsQueueRunning { get; private set; }

    public ObservableCollection<DownloadItem> Downloads { get; } = [];

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
        if (download.Status != DownloadStatus.Downloading)
        {
            Downloads.Remove(download);
        }
    }

    public async Task StartQueue()
    {
        if (IsQueueRunning)
        {
            return;
        }

        IsQueueRunning = true;

        foreach (DownloadItem download in Downloads)
        {
            if (download.Status != DownloadStatus.Pending)
            {
                continue;
            }

            download.Status = DownloadStatus.Downloading;

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
            }
        }

        IsQueueRunning = false;
    }

    public void StopQueue(bool stopCurrentDownload)
    {
        if (IsQueueRunning && stopCurrentDownload)
        {
            currentDownloadCancellationTokenSource?.Cancel();
        }

        IsQueueRunning = false;
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
