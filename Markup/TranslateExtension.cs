using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace FFmpegDownloader.Markup;

[MarkupExtensionReturnType(typeof(string))]
public sealed class TranslateExtension(string key) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        Binding binding = new()
        {
            Source = App.Localization,
            Path = new PropertyPath($"[{key}]"),
            Mode = BindingMode.OneWay
        };

        return binding.ProvideValue(serviceProvider);
    }
}
