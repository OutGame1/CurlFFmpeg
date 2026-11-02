using CurlFFmpeg.ViewModels;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace CurlFFmpeg.Views;

public partial class AddDownloadView : Page
{
    private readonly AddDownloadViewModel addDownloadViewModel;

    public AddDownloadView(AddDownloadViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        addDownloadViewModel = viewModel;
    }

    private void BrowseOutputFile(object sender, RoutedEventArgs e)
    {
        SaveFileDialog dialog = new()
        {
            Filter = "Matroska (*.mkv)|*.mkv|MP4 (*.mp4)|*.mp4",
            AddExtension = true,
            FileName = addDownloadViewModel.OutputFileName
        };

        if (dialog.ShowDialog() == true)
        {
            addDownloadViewModel.OutputPath = dialog.FileName;
        }
    }
}
