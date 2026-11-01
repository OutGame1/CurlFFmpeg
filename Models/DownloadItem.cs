using CommunityToolkit.Mvvm.ComponentModel;
using CurlFFmpeg.Enums;

namespace CurlFFmpeg.Models;

public sealed partial class DownloadItem : ObservableObject
{
    // Identifiant unique
    [ObservableProperty]
    public partial Guid Id { get; set; } = Guid.NewGuid();

    // Informations du téléchargement
    [ObservableProperty]
    public partial string Url { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string FileName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string OutputPath { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Format { get; set; } = string.Empty;

    // Informations HTTP
    [ObservableProperty]
    public partial Dictionary<string, string> Headers { get; set; } = new();

    [ObservableProperty]
    public partial string UserAgent { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Referer { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Cookies { get; set; } = string.Empty;

    // État du téléchargement
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StatusMessage))]
    public partial DownloadStatus Status { get; set; } = DownloadStatus.Pending;

    [ObservableProperty]
    public partial double Progress { get; set; }

    public string StatusMessage => App.Localization[$"DownloadStatus{Status}"];

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;
}
