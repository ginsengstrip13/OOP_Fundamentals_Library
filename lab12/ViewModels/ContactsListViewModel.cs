using System.Collections.ObjectModel;
using System.Windows.Input;
using lab12.Models;
using lab12.Services;

namespace lab12.ViewModels;

/// <summary>
/// ViewModel экрана списка контактов. Это бывшая MainViewModel телефонной книги.
/// </summary>
public sealed class ContactsListViewModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigation;
    private readonly IContactsRepository _contactsRepository;
    private string _name = string.Empty;
    private string _phone = string.Empty;
    private Contact? _selectedContact;
    private string _validationMessage = "Введите имя и телефон в формате +7XXXXXXXXXX или XXXXXXXXXX.";

    public ContactsListViewModel(
        IDialogService dialogService,
        INavigationService navigation,
        IContactsRepository contactsRepository)
    {
        // Все зависимости приходят через Constructor Injection.
        // ViewModel не создает сервисы сама и не зависит от конкретных WPF-окон.
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        _contactsRepository = contactsRepository ?? throw new ArgumentNullException(nameof(contactsRepository));

        // Команды используются в XAML через Binding, поэтому в ContactsListView.xaml нет обработчиков Click.
        AddCommand = new RelayCommand(AddContact, CanAddContact);
        DeleteCommand = new RelayCommand<Contact>(DeleteContact, CanUseSelectedContact);
        EditContactCommand = new RelayCommand<Contact>(EditContact, CanUseSelectedContact);
    }

    // Коллекция хранится в Singleton-репозитории.
    // Это сохраняет контакты при возврате на экран, хотя ContactsListViewModel зарегистрирована как Transient.
    public ObservableCollection<Contact> Contacts => _contactsRepository.Contacts;

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

    public string ValidationMessage
    {
        get => _validationMessage;
        private set => Set(ref _validationMessage, value);
    }

    public ICommand AddCommand { get; }

    public ICommand DeleteCommand { get; }

    public ICommand EditContactCommand { get; }

    private void AddContact()
    {
        try
        {
            // Перед добавлением нормализуем телефон и проверяем уникальность номера.
            var normalizedName = Name.Trim();
            var normalizedPhone = Phone.Trim();

            if (Contacts.Any(contact => contact.Phone.Equals(normalizedPhone, StringComparison.OrdinalIgnoreCase)))
            {
                _dialogService.ShowWarning("Контакт с таким номером уже существует!");
                ValidationMessage = "Дубликат номера не был добавлен.";
                return;
            }

            var contact = new Contact
            {
                Name = normalizedName,
                Phone = normalizedPhone
            };

            if (!contact.Validate(out var errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            _contactsRepository.Add(contact);

            // После успешного добавления очищаем форму ввода и показываем обратную связь.
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

    private void EditContact(Contact? contact)
    {
        if (contact is not null)
        {
            // ViewModel-First переход: передаем не View, а выбранную модель Contact.
            // ContactEditViewModel получит ее через INavigationAware.OnNavigatedTo.
            _navigation.NavigateTo<ContactEditViewModel>(contact);
        }
    }

    private void DeleteContact(Contact? contact)
    {
        if (contact is null)
        {
            return;
        }

        var confirmed = _dialogService.ShowConfirmation(
            $"Удалить контакт \"{contact.Name}\" с номером {contact.Phone}?");

        // Диалоговое окно остается задачей IDialogService, а навигация остается задачей INavigationService.
        if (!confirmed)
        {
            ValidationMessage = "Удаление отменено пользователем.";
            return;
        }

        _contactsRepository.Remove(contact);

        if (SelectedContact == contact)
        {
            SelectedContact = null;
        }

        ValidationMessage = "Выбранный контакт удален.";
    }

    private bool CanAddContact()
    {
        return !string.IsNullOrWhiteSpace(Name) && Contact.IsPhoneValid(Phone);
    }

    private static bool CanUseSelectedContact(Contact? contact)
    {
        return contact is not null;
    }

    private void RefreshValidationState()
    {
        // При каждом изменении ввода обновляем подсказку и просим WPF перепроверить CanExecute команд.
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
