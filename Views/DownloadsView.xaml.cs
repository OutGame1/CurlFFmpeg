using CurlFFmpeg.ViewModels;
using System.Windows.Controls;

namespace CurlFFmpeg.Views;

public partial class DownloadsView : Page
{
    public DownloadsView(DownloadsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
