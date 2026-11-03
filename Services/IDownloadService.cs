using FFmpegDownloader.Models;
using System.Collections.ObjectModel;

namespace FFmpegDownloader.Services;

public interface IDownloadService
{
    ObservableCollection<DownloadItem> Downloads { get; }

    void RemoveDownload(DownloadItem download);
}
