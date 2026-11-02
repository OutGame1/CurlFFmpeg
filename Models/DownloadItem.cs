using CommunityToolkit.Mvvm.ComponentModel;
using CurlFFmpeg.Enums;

namespace CurlFFmpeg.Models;

public sealed partial class DownloadItem : ObservableObject
{
    // Identifiant unique
    public readonly Guid Id = Guid.NewGuid();

    // Informations du téléchargement
    [ObservableProperty]
    public partial string Url { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string FileName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string OutputPath { get; set; } = string.Empty;

    // Informations HTTP
    [ObservableProperty]
    public partial Dictionary<string, string> Headers { get; set; } = new();

    // État du téléchargement
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StatusMessage))]
    public partial DownloadStatus Status { get; set; } = DownloadStatus.Pending;

    public string StatusMessage => App.Localization[$"DownloadStatus{Status}"];

    [ObservableProperty]
    public partial double Progress { get; set; }
}
