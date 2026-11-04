using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FFmpegDownloader.Services;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace FFmpegDownloader.ViewModels;

public sealed partial class AddDownloadViewModel(IDownloadService downloadService,
                                                 ISnackbarService snackbarService) : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(AddDownloadViewModel), nameof(ValidateVideoUrl))]
    public partial string VideoUrl { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [CustomValidation(typeof(AddDownloadViewModel), nameof(ValidateOutputPath))]
    public partial string OutputPath { get; set; } = string.Empty;

    public string OutputFileName => Path.GetFileName(OutputPath);

    public static ValidationResult? ValidateVideoUrl(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ValidationResult(App.Localization["RequiredUrl"]);
        }

        if (!Uri.TryCreate(value.Trim(), UriKind.Absolute, out Uri? uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps) ||
            string.IsNullOrWhiteSpace(uri.Host))
        {
            return new ValidationResult(App.Localization["InvalidUrl"]);
        }

        return ValidationResult.Success;
    }

    public static ValidationResult? ValidateOutputPath(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ValidationResult(App.Localization["OutputPathRequired"]);
        }

        return ValidationResult.Success;
    }

    [RelayCommand]
    private void AddDownload()
    {
        ValidateAllProperties();

        if (HasErrors)
        {
            return;
        }

        downloadService.AddDownload(VideoUrl.Trim(),
                                    OutputFileName,
                                    OutputPath);

        ShowAddedDownloadSuccessSnackbar();

        ResetFields();
    }

    private void ResetFields()
    {
        VideoUrl = string.Empty;
        OutputPath = string.Empty;
        ClearErrors();
    }

    private void ShowAddedDownloadSuccessSnackbar()
    {
        snackbarService.Show(OutputFileName,
                             App.Localization["DownloadAddedDescription"],
                             ControlAppearance.Success,
                             null,
                             TimeSpan.FromSeconds(7));
    }
}
