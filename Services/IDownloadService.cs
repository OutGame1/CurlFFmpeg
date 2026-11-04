using FFmpegDownloader.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FFmpegDownloader.Services;

public interface IDownloadService : INotifyPropertyChanged
{
    ObservableCollection<DownloadItem> Downloads { get; }

    void AddDownload(string url, string fileName, string outputPath);

    void RemoveDownload(DownloadItem download);

    Task StartQueue();

    void StopQueue(bool stopCurrentDownload);
}
