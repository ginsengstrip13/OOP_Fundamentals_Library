using System.Windows.Input;

namespace lab14.ViewModels;

/// <summary>
/// Команда без параметра для MVVM-привязок из XAML.
/// </summary>
public sealed class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        // Если условие доступности не задано, команда считается всегда доступной.
        return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        // Execute дополнительно проверяет CanExecute, чтобы команда не выполнилась в запрещенном состоянии.
        if (CanExecute(parameter))
        {
            _execute.Invoke();
        }
    }

    public event EventHandler? CanExecuteChanged
    {
        // CommandManager сам инициирует перепроверку команд при изменениях в UI.
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

/// <summary>
/// Команда с параметром, например выбранным контактом из DataGrid.
/// </summary>
public sealed class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;
    private readonly Predicate<T?>? _canExecute;

    public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        // Параметр команды приходит из XAML, например SelectedContact из DataGrid.
        var typedParameter = parameter is T value ? value : default;
        return _canExecute?.Invoke(typedParameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        // Приводим object? к ожидаемому типу, чтобы ViewModel работала с Contact?, а не с object.
        var typedParameter = parameter is T value ? value : default;

        if (CanExecute(typedParameter))
        {
            _execute.Invoke(typedParameter);
        }
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
