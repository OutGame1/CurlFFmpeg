using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace FFmpegDownloader.Dialogs;

public sealed class StopQueueConfirmationDialog : ContentDialog
{
    public readonly CheckBox StopCurrentDownloadCheckBox;

    public StopQueueConfirmationDialog(bool isStopCurrentDownloadCheckboxVisible)
    {
        SetResourceReference(StyleProperty, typeof(ContentDialog));
        Title = "ATRADUIRE:Arrêter la file d'attente";
        PrimaryButtonText = App.Localization["Stop"];
        PrimaryButtonAppearance = ControlAppearance.Danger;
        CloseButtonText = App.Localization["Cancel"];
        DefaultButton = ContentDialogButton.Close;

        StopCurrentDownloadCheckBox = new CheckBox
        {
            Content = "ATRADUIRE:Arrêter également le téléchargement actuel",
            Margin = new Thickness(0, 16, 0, 0),
            Visibility = isStopCurrentDownloadCheckboxVisible ? Visibility.Visible : Visibility.Collapsed
        };

        Content = new StackPanel
        {
            Children =
            {
                new Wpf.Ui.Controls.TextBlock
                {
                    Text = "ATRADUIRE:Souhaitez-vous vraiment arrêter la file d'attente ?",
                    TextWrapping = TextWrapping.Wrap
                },
                StopCurrentDownloadCheckBox
            }
        };
    }
}
