using CurlFFmpeg.Models;
using System.Collections.ObjectModel;

namespace CurlFFmpeg.Services;

public interface IDownloadService
{
    ObservableCollection<DownloadItem> Downloads { get; }

    void RemoveDownload(DownloadItem download);
}
