using FFmpegDownloader.ViewModels;
using FFmpegDownloader.Views;
using System.Windows;
using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Controls;

namespace FFmpegDownloader;

public partial class MainWindow : FluentWindow
{
    public MainWindow(MainViewModel mainViewModel,
                      INavigationViewPageProvider pageProvider,
                      IContentDialogService dialogService,
                      ISnackbarService snackbarService)
    {
        InitializeComponent();

        DataContext = mainViewModel;

        NavigationControl.SetPageProviderService(pageProvider);

        dialogService.SetDialogHost(RootContentDialogHost);
        snackbarService.SetSnackbarPresenter(RootSnackbarPresenter);

        void NavigateToAddDownload(object sender, RoutedEventArgs e)
        {
            NavigationControl.Navigate(typeof(AddDownloadView));
            Loaded -= NavigateToAddDownload;
        }

        Loaded += NavigateToAddDownload;
    }
}
