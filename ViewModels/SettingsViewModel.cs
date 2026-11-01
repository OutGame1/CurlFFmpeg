using CommunityToolkit.Mvvm.ComponentModel;

namespace CurlFFmpeg.ViewModels;

public sealed partial class SettingsViewModel : ObservableObject
{
    public string SelectedCulture
    {
        get => App.Localization.CurrentCulture.Name;
        set => SetProperty(App.Localization.CurrentCulture.Name,
                           value,
                           culture => App.Localization.SetCulture(culture));
    }
}
