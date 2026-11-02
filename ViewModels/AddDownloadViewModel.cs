using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurlFFmpeg.Enums;
using CurlFFmpeg.Models;
using CurlFFmpeg.Services;

namespace CurlFFmpeg.ViewModels;

public sealed partial class AddDownloadViewModel(IDownloadService downloadService) : ObservableObject
{
    private readonly IDownloadService downloadService = downloadService;

    [ObservableProperty]
    public partial string PlaylistUrl { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string OutputFile { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Format { get; set; } = "mkv";

    public string[] AvailableFormats { get; } =
    [
        "mkv",
        "mp4"
    ];

    [RelayCommand]
    private void AddDownload()
    {
        DownloadItem item = new()
        {
            Url = PlaylistUrl,
            OutputPath = OutputFile,
            Format = Format,
            Status = DownloadStatus.Pending
        };

        downloadService.Downloads.Add(item);
    }
}
