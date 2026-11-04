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
        Title = App.Localization["StopQueue"];
        PrimaryButtonText = App.Localization["Stop"];
        PrimaryButtonAppearance = ControlAppearance.Danger;
        CloseButtonText = App.Localization["Cancel"];
        DefaultButton = ContentDialogButton.Close;

        StopCurrentDownloadCheckBox = new CheckBox
        {
            Content = App.Localization["StopCurrentDownloadConfirmation"],
            Margin = new Thickness(0, 16, 0, 0),
            Visibility = isStopCurrentDownloadCheckboxVisible ? Visibility.Visible : Visibility.Collapsed
        };

        Content = new StackPanel
        {
            Children =
            {
                new Wpf.Ui.Controls.TextBlock
                {
                    Text = App.Localization["StopQueueConfirmationMessage"],
                    TextWrapping = TextWrapping.Wrap
                },
                StopCurrentDownloadCheckBox
            }
        };
    }
}
