using Wpf.Ui.Controls;

namespace FFmpegDownloader.Dialogs;

public sealed class RemoveDownloadConfirmationDialog : ContentDialog
{
    public RemoveDownloadConfirmationDialog(string fileName)
    {
        SetResourceReference(StyleProperty, typeof(ContentDialog));
        Title = App.Localization["ConfirmDeletion"];
        Content = $"{App.Localization["Delete"]} « {fileName} » ?";
        PrimaryButtonText = App.Localization["Delete"];
        PrimaryButtonAppearance = ControlAppearance.Danger;
        CloseButtonText = App.Localization["Cancel"];
        DefaultButton = ContentDialogButton.Close;
    }
}
