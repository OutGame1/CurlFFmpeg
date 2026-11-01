using CurlFFmpeg.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace CurlFFmpeg;

public partial class MainWindow : FluentWindow
{
    public MainWindow(MainViewModel mainViewModel,
                      INavigationViewPageProvider pageProvider,
                      IContentDialogService dialogService)
    {
        InitializeComponent();

        DataContext = mainViewModel;

        NavigationControl.SetPageProviderService(pageProvider);

        dialogService.SetDialogHost(RootContentDialogHost);
    }
}
