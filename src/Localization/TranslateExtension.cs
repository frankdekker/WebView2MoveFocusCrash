#nullable enable
using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Localization;

public class TranslateExtension : MarkupExtension
{
    private string key;

    public TranslateExtension(string key)
    {
        this.key = key;
    }

    [ConstructorArgument("key")]
    public string Key
    {
        get { return key; }
        set { key = value; }
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new Binding("Value") { Source = new TranslationData(key) };
        return binding.ProvideValue(serviceProvider);
    }
}