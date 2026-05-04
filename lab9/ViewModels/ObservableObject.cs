using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace lab9.ViewModels;

/// <summary>
/// Базовый класс MVVM для объектов, которые должны сообщать View об изменении свойств.
/// В WPF это нужно механизму Data Binding: когда ViewModel меняет значение свойства,
/// интерфейс автоматически получает уведомление и обновляет связанный элемент управления.
/// </summary>
public abstract class ObservableObject : INotifyPropertyChanged
{
    /// <summary>
    /// Событие стандартного интерфейса INotifyPropertyChanged.
    /// На него подписывается WPF Binding, когда свойство используется в XAML-разметке.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Вызывает событие изменения свойства.
    /// Атрибут CallerMemberName позволяет не передавать имя свойства вручную в большинстве случаев.
    /// </summary>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Универсальный помощник для сеттеров свойств ViewModel и Model.
    /// Метод меняет поле только при реальном изменении значения и затем уведомляет View.
    /// </summary>
    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

