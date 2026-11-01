using CurlFFmpeg.ViewModels;
using System.Windows.Controls;

namespace CurlFFmpeg.Views;

public partial class AddDownloadView : Page
{
    public AddDownloadView(AddDownloadViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
