using CurlFFmpeg.ViewModels;
using System.Windows.Controls;

namespace CurlFFmpeg.Views;

public partial class SettingsView : Page
{
    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
