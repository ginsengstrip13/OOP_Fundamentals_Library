using System.Collections.ObjectModel;
using System.Windows.Input;
using lab10.Models;
using lab10.Services;

namespace lab10.ViewModels;

/// <summary>
/// ViewModel телефонной книги.
/// Зависимость от IDialogService внедряется через конструктор, поэтому ViewModel не создает MessageBox напрямую.
/// </summary>
public sealed class MainViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private string _name = string.Empty;
    private string _phone = string.Empty;
    private Contact? _selectedContact;
    private string _validationMessage = "Введите имя и телефон в формате +7XXXXXXXXXX или XXXXXXXXXX.";

    /// <summary>
    /// ObservableCollection автоматически сообщает DataGrid о добавлении и удалении контактов.
    /// </summary>
    public ObservableCollection<Contact> Contacts { get; } = new();

    /// <summary>
    /// Свойство ввода имени. TextBox обновляет его через TwoWay Binding.
    /// </summary>
    public string Name
    {
        get => _name;
        set
        {
            if (Set(ref _name, value))
            {
                RefreshValidationState();
            }
        }
    }

    /// <summary>
    /// Свойство ввода телефона. TextBox обновляет его при каждом изменении текста.
    /// </summary>
    public string Phone
    {
        get => _phone;
        set
        {
            if (Set(ref _phone, value))
            {
                RefreshValidationState();
            }
        }
    }

    /// <summary>
    /// Контакт, выбранный в DataGrid. DeleteCommand получает его как параметр.
    /// </summary>
    public Contact? SelectedContact
    {
        get => _selectedContact;
        set
        {
            if (Set(ref _selectedContact, value))
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }

    /// <summary>
    /// Сообщение под полями ввода показывает состояние проверки данных.
    /// </summary>
    public string ValidationMessage
    {
        get => _validationMessage;
        private set => Set(ref _validationMessage, value);
    }

    /// <summary>
    /// Команда добавления контакта без параметра.
    /// </summary>
    public ICommand AddCommand { get; }

    /// <summary>
    /// Команда удаления контакта с параметром, выбранным в DataGrid.
    /// </summary>
    public ICommand DeleteCommand { get; }

    /// <summary>
    /// Constructor Injection: контейнер DI передает реализацию IDialogService в момент создания ViewModel.
    /// </summary>
    public MainViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        AddCommand = new RelayCommand(AddContact, CanAddContact);
        DeleteCommand = new RelayCommand<Contact>(DeleteContact, CanDeleteContact);
    }

    /// <summary>
    /// Добавляет контакт, предварительно проверяя дубликат по номеру телефона.
    /// Результат операции сообщается пользователю через IDialogService.
    /// </summary>
    private void AddContact()
    {
        try
        {
            var normalizedPhone = Phone.Trim();

            if (Contacts.Any(contact => contact.Phone.Equals(normalizedPhone, StringComparison.OrdinalIgnoreCase)))
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует!");
                ValidationMessage = "Дубликат номера не был добавлен.";
                return;
            }

            var contact = new Contact(Name, normalizedPhone);
            Contacts.Add(contact);

            Name = string.Empty;
            Phone = string.Empty;
            ValidationMessage = "Контакт добавлен. Можно ввести следующий.";
            _dialogService.ShowInfo("Контакт успешно добавлен.");
        }
        catch (ArgumentException exception)
        {
            ValidationMessage = exception.Message;
            _dialogService.ShowError(exception.Message);
        }
    }

    /// <summary>
    /// Кнопка Добавить активна только при непустом имени и корректном номере телефона.
    /// </summary>
    private bool CanAddContact()
    {
        return !string.IsNullOrWhiteSpace(Name) && Contact.IsPhoneValid(Phone);
    }

    /// <summary>
    /// Запрашивает подтверждение удаления через сервис диалогов.
    /// Если пользователь отвечает Нет или закрывает окно, контакт остается в коллекции.
    /// </summary>
    private void DeleteContact(Contact? contact)
    {
        if (contact is null)
        {
            return;
        }

        var confirmed = _dialogService.ShowConfirmation(
            $"Удалить контакт \"{contact.Name}\" с номером {contact.Phone}?");

        if (!confirmed)
        {
            ValidationMessage = "Удаление отменено пользователем.";
            return;
        }

        Contacts.Remove(contact);

        if (SelectedContact == contact)
        {
            SelectedContact = null;
        }

        ValidationMessage = "Выбранный контакт удален.";
    }

    /// <summary>
    /// Кнопка Удалить доступна только тогда, когда DataGrid передал выбранный контакт.
    /// </summary>
    private static bool CanDeleteContact(Contact? contact)
    {
        return contact is not null;
    }

    /// <summary>
    /// Обновляет подсказку и просит WPF заново проверить CanExecute у команд.
    /// </summary>
    private void RefreshValidationState()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            ValidationMessage = "Введите имя контакта.";
        }
        else if (string.IsNullOrWhiteSpace(Phone))
        {
            ValidationMessage = "Введите номер телефона.";
        }
        else if (!Contact.IsPhoneValid(Phone))
        {
            ValidationMessage = "Телефон должен иметь формат +7XXXXXXXXXX или XXXXXXXXXX.";
        }
        else
        {
            ValidationMessage = "Данные корректны. Контакт можно добавить.";
        }

        CommandManager.InvalidateRequerySuggested();
    }
}
