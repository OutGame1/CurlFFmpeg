using CurlFFmpeg.Models;
using System.Collections.ObjectModel;

namespace CurlFFmpeg.Services;

public sealed class DownloadService : IDownloadService
{
    public ObservableCollection<DownloadItem> Downloads { get; } = [];

    public void RemoveDownload(DownloadItem download)
    {
        Downloads.Remove(download);
    }
}
