using CurlFFmpeg.Resources;
using System.ComponentModel;
using System.Globalization;

namespace CurlFFmpeg;

public sealed class Localization : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public CultureInfo CurrentCulture { get; private set; } =
        CultureInfo.GetCultureInfo("fr-FR");

    public string this[string key] =>
        Strings.ResourceManager.GetString(key, CurrentCulture) ?? $"ATRADUIRE:{key}";

    public void SetCulture(string cultureName)
    {
        CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);

        if (CurrentCulture.Equals(culture))
        {
            return;
        }

        CurrentCulture = culture;

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}