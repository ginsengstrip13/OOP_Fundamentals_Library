using System.Collections.ObjectModel;
using System.Windows.Input;
using lab9.Models;

namespace lab9.ViewModels;

/// <summary>
/// ViewModel в паттерне MVVM.
/// MainViewModel хранит состояние окна, список контактов и команды пользователя.
/// View привязывается к этим свойствам через Data Binding и не содержит бизнес-логики.
/// </summary>
public sealed class MainViewModel : ObservableObject
{
    private string _name = string.Empty;
    private string _phone = string.Empty;
    private Contact? _selectedContact;
    private string _validationMessage = "Введите имя и телефон в формате +7XXXXXXXXXX или XXXXXXXXXX.";

    /// <summary>
    /// ObservableCollection автоматически сообщает DataGrid о добавлении и удалении строк.
    /// Поэтому после Contacts.Add(...) или Contacts.Remove(...) таблица обновляется сама.
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
    /// Контакт, выбранный в DataGrid.
    /// DeleteCommand использует это значение как параметр для удаления.
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
    /// Сообщение под полями ввода. Оно помогает пользователю понять, почему AddCommand недоступна.
    /// </summary>
    public string ValidationMessage
    {
        get => _validationMessage;
        private set => Set(ref _validationMessage, value);
    }

    /// <summary>
    /// Команда добавления контакта без параметра, как требуется в задании.
    /// </summary>
    public ICommand AddCommand { get; }

    /// <summary>
    /// Команда удаления контакта с параметром. Параметр приходит из Binding CommandParameter.
    /// </summary>
    public ICommand DeleteCommand { get; }

    /// <summary>
    /// В конструкторе ViewModel инициализируются команды.
    /// View получает готовые ICommand-объекты через DataContext.
    /// </summary>
    public MainViewModel()
    {
        AddCommand = new RelayCommand(AddContact, CanAddContact);
        DeleteCommand = new RelayCommand<Contact>(DeleteContact, CanDeleteContact);
    }

    /// <summary>
    /// Создает модель Contact и добавляет ее в коллекцию.
    /// После успешного добавления поля ввода очищаются, а таблица обновляется через ObservableCollection.
    /// </summary>
    private void AddContact()
    {
        try
        {
            var contact = new Contact(Name, Phone);
            Contacts.Add(contact);

            Name = string.Empty;
            Phone = string.Empty;
            ValidationMessage = "Контакт добавлен. Можно ввести следующий.";
        }
        catch (ArgumentException exception)
        {
            ValidationMessage = exception.Message;
        }
    }

    /// <summary>
    /// Определяет доступность кнопки Добавить.
    /// Кнопка активна только при непустом имени и корректном номере телефона.
    /// </summary>
    private bool CanAddContact()
    {
        return !string.IsNullOrWhiteSpace(Name) && Contact.IsPhoneValid(Phone);
    }

    /// <summary>
    /// Удаляет контакт, переданный из View через CommandParameter.
    /// ViewModel работает с моделью данных, а не с конкретной строкой DataGrid.
    /// </summary>
    private void DeleteContact(Contact? contact)
    {
        if (contact is null)
        {
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
    /// Это связывает ввод в TextBox с доступностью кнопки Добавить.
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

