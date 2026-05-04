using System.Windows.Input;

namespace lab9.ViewModels;

/// <summary>
/// Команда без параметра для MVVM.
/// View вызывает ее через Binding из XAML, а ViewModel получает управление без обработчика Click в code-behind.
/// </summary>
public sealed class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    /// <summary>
    /// execute - действие, которое выполняется при нажатии кнопки.
    /// canExecute - условие доступности команды, например корректность введенных данных.
    /// </summary>
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// WPF вызывает этот метод, чтобы понять, активна ли кнопка, связанная с командой.
    /// </summary>
    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    /// <summary>
    /// Выполнение команды. В данном проекте AddCommand добавляет новый контакт в коллекцию.
    /// </summary>
    public void Execute(object? parameter)
    {
        if (CanExecute(parameter))
        {
            _execute.Invoke();
        }
    }

    /// <summary>
    /// CommandManager автоматически просит WPF перепроверять CanExecute при изменении состояния UI.
    /// Это позволяет кнопкам включаться и выключаться при вводе текста.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

/// <summary>
/// Команда с параметром для MVVM.
/// В лабораторной работе она используется для удаления контакта, переданного из DataGrid.
/// </summary>
public sealed class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;
    private readonly Predicate<T?>? _canExecute;

    /// <summary>
    /// execute принимает параметр команды, например выбранный Contact.
    /// canExecute проверяет, можно ли выполнить действие для текущего параметра.
    /// </summary>
    public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    /// <summary>
    /// Преобразует object-параметр WPF к ожидаемому типу T и проверяет доступность команды.
    /// </summary>
    public bool CanExecute(object? parameter)
    {
        var typedParameter = parameter is T value ? value : default;
        return _canExecute?.Invoke(typedParameter) ?? true;
    }

    /// <summary>
    /// Выполняет команду с параметром. Для DeleteCommand параметром является выбранный контакт.
    /// </summary>
    public void Execute(object? parameter)
    {
        var typedParameter = parameter is T value ? value : default;

        if (CanExecute(typedParameter))
        {
            _execute.Invoke(typedParameter);
        }
    }

    /// <summary>
    /// Подписка на RequerySuggested нужна, чтобы кнопка удаления стала активной сразу после выбора строки.
    /// </summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}

