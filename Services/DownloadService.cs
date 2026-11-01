using CurlFFmpeg.Enums;
using CurlFFmpeg.Models;
using System.Collections.ObjectModel;

namespace CurlFFmpeg.Services;

public sealed class DownloadService : IDownloadService
{
    public ObservableCollection<DownloadItem> Downloads { get; } = [];

    public DownloadService()
    {
        // Données temporaires pour tester l'interface
        Downloads.Add(new DownloadItem
        {
            FileName = "Film_exemple.mkv",
            Url = "https://exemple.com/video.m3u8",
            Progress = 67,
            Status = DownloadStatus.Downloading
        });

        Downloads.Add(new DownloadItem
        {
            FileName = "Documentaire.mkv",
            Url = "https://exemple.com/documentaire.m3u8",
            Progress = 100,
            Status = DownloadStatus.Completed
        });

        Downloads.Add(new DownloadItem
        {
            FileName = "Autre_video.mkv",
            Url = "https://exemple.com/autre-video.m3u8",
            Progress = 0,
            Status = DownloadStatus.Pending
        });
    }

    public void RemoveDownload(DownloadItem download)
    {
        Downloads.Remove(download);
    }
}
