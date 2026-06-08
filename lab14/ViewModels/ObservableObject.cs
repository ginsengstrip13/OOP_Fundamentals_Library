using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace lab14.ViewModels;

/// <summary>
/// Базовый класс MVVM для объектов, которые уведомляют View об изменении свойств.
/// </summary>
public abstract class ObservableObject : INotifyPropertyChanged
{
    // WPF Binding подписывается на это событие и обновляет элементы интерфейса.
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        // CallerMemberName подставляет имя свойства автоматически,
        // поэтому в сеттерах не нужно писать строковые литералы.
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        // Если значение не изменилось, уведомление не отправляется.
        // Это защищает интерфейс от лишних перерисовок.
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        // Меняем поле и сообщаем View, что связанное свойство обновилось.
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
