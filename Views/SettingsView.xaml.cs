using FFmpegDownloader.ViewModels;
using System.Windows.Controls;

namespace FFmpegDownloader.Views;

public partial class SettingsView : Page
{
    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
