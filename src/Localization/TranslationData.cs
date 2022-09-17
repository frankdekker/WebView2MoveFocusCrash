#nullable enable
using System;
using System.ComponentModel;
using System.Windows;

namespace Localization;

public class TranslationData : IWeakEventListener, INotifyPropertyChanged, IDisposable
{
    private readonly string key;
    private readonly bool translate = true;
    public event PropertyChangedEventHandler? PropertyChanged;

    public TranslationData(string key)
    {
        this.key = key;
        TranslationManager.Instance.LanguageChanged += OnLanguageChanged;
    }
    
    public TranslationData(string key, bool translate)
    {
        this.key = key;
        this.translate = translate;
        TranslationManager.Instance.LanguageChanged += OnLanguageChanged;
    }

    public object Value
    {
        get => translate ? TranslationManager.Instance.Translate(key) : key;
        set
        {
            // do nothing
        }
    }

    public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
    {
        if (managerType != typeof(TranslationManager)) {
            return false;
        }

        OnLanguageChanged(sender, e);
        return true;
    }

    ~TranslationData()
    {
        Dispose(false);
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing) {
            TranslationManager.Instance.LanguageChanged -= OnLanguageChanged;
        }
    }

    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
    }
}