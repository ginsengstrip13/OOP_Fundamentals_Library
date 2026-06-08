using System.Collections.ObjectModel;
using System.Windows.Input;
using lab13.Data;
using lab13.Models;
using lab13.Services;

namespace lab13.ViewModels;

public sealed class ContactsListViewModel : ObservableObject
{
    private readonly ApplicationContext _context;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigation;
    private string _name = string.Empty;
    private string _phone = string.Empty;
    private Contact? _selectedContact;
    private string _validationMessage = "Введите имя и телефон в формате +7XXXXXXXXXX или XXXXXXXXXX.";

    public ContactsListViewModel(
        ApplicationContext context,
        IDialogService dialogService,
        INavigationService navigation)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

        Contacts = new ObservableCollection<Contact>();
        AddCommand = new RelayCommand(AddContact, CanAddContact);
        DeleteCommand = new RelayCommand<Contact>(DeleteContact, CanUseSelectedContact);
        EditContactCommand = new RelayCommand<Contact>(EditContact, CanUseSelectedContact);

        ReloadContacts();
    }

    public ObservableCollection<Contact> Contacts { get; }

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

    private void ReloadContacts()
    {
        Contacts.Clear();

        foreach (var contact in _context.Contacts.OrderBy(contact => contact.Name).ToList())
        {
            Contacts.Add(contact);
        }

        CommandManager.InvalidateRequerySuggested();
    }

    private void AddContact()
    {
        try
        {
            var normalizedName = Name.Trim();
            var normalizedPhone = Phone.Trim();

            if (_context.Contacts.Any(contact => contact.Phone == normalizedPhone))
            {
                ValidationMessage = "Контакт с таким номером уже существует.";
                _dialogService.ShowWarning(ValidationMessage);
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

            _context.Contacts.Add(contact);
            _context.SaveChanges();
            ReloadContacts();

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

        if (!confirmed)
        {
            ValidationMessage = "Удаление отменено пользователем.";
            return;
        }

        _context.Contacts.Remove(contact);
        _context.SaveChanges();

        if (SelectedContact == contact)
        {
            SelectedContact = null;
        }

        ReloadContacts();
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
