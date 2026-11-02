using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurlFFmpeg.Models;
using CurlFFmpeg.Services;
using System.IO;

namespace CurlFFmpeg.ViewModels;

public sealed partial class AddDownloadViewModel(IDownloadService downloadService) : ObservableObject
{
    private readonly IDownloadService downloadService = downloadService;

    [ObservableProperty]
    public partial string PlaylistUrl { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string OutputPath { get; set; } = string.Empty;

    public string OutputFileName => Path.GetFileName(OutputPath);

    [RelayCommand]
    private void AddDownload()
    {
        if (!Uri.TryCreate(PlaylistUrl, UriKind.Absolute, out Uri? uri) ||
           (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
           string.IsNullOrWhiteSpace(OutputPath))
        {
            // Invalid URL or output path, do not add the download
            // TODO: Show an error message to the user
            return;
        }

        DownloadItem item = new()
        {
            Url = PlaylistUrl.Trim(),
            FileName = OutputFileName,
            OutputPath = OutputPath
        };

        downloadService.Downloads.Add(item);
    }
}
