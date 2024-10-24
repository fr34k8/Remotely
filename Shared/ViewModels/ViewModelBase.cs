﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Remotely.Shared.ViewModels;

public interface IInvokePropertyChanged
{
    void InvokePropertyChanged(string propertyName = "");
}

public class ViewModelBase : INotifyPropertyChanged, IInvokePropertyChanged
{
    private readonly Dictionary<string, object?> _propertyBackingDictionary = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public void InvokePropertyChanged(string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected T? Get<T>([CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (_propertyBackingDictionary.TryGetValue(propertyName, out var value) &&
            value is T typedValue)
        {
            return typedValue;
        }

        return default;
    }

    protected bool Set<T>(T? newValue, [CallerMemberName] string? propertyName = null)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName));
        }

        if (EqualityComparer<T>.Default.Equals(newValue, Get<T>(propertyName)))
        {
            return false;
        }

        _propertyBackingDictionary[propertyName] = newValue;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
