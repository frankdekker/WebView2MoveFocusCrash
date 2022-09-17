#nullable enable
using System;
using System.Globalization;
using System.Threading;

namespace Localization;

public class TranslationManager
{
    public static CultureInfo? DefaultCulture = null;
    private static TranslationManager? _translationManager;

    public event EventHandler? LanguageChanged;
    private CultureInfo culture = Thread.CurrentThread.CurrentUICulture;

    public static TranslationManager Instance => _translationManager ??= new TranslationManager();
        
    public CultureInfo CurrentLanguage
    {
        set
        {
            if (Equals(value, culture) == false) {
                culture = value;
                OnLanguageChanged();
            }
        }
    }

    public void SetLanguageCode(string languageCode)
    {
        if (languageCode.ToLower() == "nl") {
            CurrentLanguage = CultureInfo.GetCultureInfo("nl-NL");
        } else {
            CurrentLanguage = DefaultCulture ?? CultureInfo.GetCultureInfo("en-US");
        }
    }

    public string Translate(string key, string? defaultValue = null)
    {
        return defaultValue ?? key;
    }

    private void OnLanguageChanged()
    {
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        LanguageChanged?.Invoke(this, EventArgs.Empty);
    }
}