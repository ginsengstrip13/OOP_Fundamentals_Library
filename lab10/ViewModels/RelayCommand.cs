using System.Windows.Input;

namespace lab10.ViewModels;

/// <summary>
/// Команда без параметра для MVVM.
/// View вызывает ее через Binding, поэтому в code-behind не нужны обработчики Click.
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
        return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        if (CanExecute(parameter))
        {
            _execute.Invoke();
        }
    }

    /// <summary>
    /// CommandManager просит WPF перепроверять CanExecute при изменении состояния интерфейса.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

/// <summary>
/// Команда с параметром для MVVM.
/// В приложении используется для удаления контакта, выбранного в DataGrid.
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
        var typedParameter = parameter is T value ? value : default;
        return _canExecute?.Invoke(typedParameter) ?? true;
    }

    public void Execute(object? parameter)
    {
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
