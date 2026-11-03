using FFmpegDownloader.ViewModels;
using System.Windows.Controls;

namespace FFmpegDownloader.Views;

public partial class DownloadsView : Page
{
    public DownloadsView(DownloadsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
