using System.Windows.Input;
using lab14.Data;
using lab14.Models;
using lab14.Services;
using Microsoft.EntityFrameworkCore;

namespace lab14.ViewModels;

public sealed class ContactEditViewModel : ObservableObject, INavigationAware
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigation;
    private Contact? _contact;
    private string _editName = string.Empty;
    private string _editPhone = string.Empty;
    private string _validationMessage = "Измените данные контакта и нажмите Сохранить.";

    public ContactEditViewModel(
        IDbContextFactory<ApplicationContext> contextFactory,
        IDialogService dialogService,
        INavigationService navigation)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

        SaveCommand = new RelayCommand(Save, CanSave);
        CancelCommand = new RelayCommand(() => _navigation.NavigateTo<ContactsListViewModel>());
    }

    public string EditName
    {
        get => _editName;
        set
        {
            if (Set(ref _editName, value))
            {
                RefreshValidationState();
            }
        }
    }

    public string EditPhone
    {
        get => _editPhone;
        set
        {
            if (Set(ref _editPhone, value))
            {
                RefreshValidationState();
            }
        }
    }

    public string ValidationMessage
    {
        get => _validationMessage;
        private set => Set(ref _validationMessage, value);
    }

    public ICommand SaveCommand { get; }

    public ICommand CancelCommand { get; }

    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is not Contact contact)
        {
            ValidationMessage = "Контакт для редактирования не выбран.";
            return;
        }

        _contact = contact;
        EditName = contact.Name;
        EditPhone = contact.Phone;
    }

    private void Save()
    {
        if (_contact is null)
        {
            _navigation.NavigateTo<ContactsListViewModel>();
            return;
        }

        var normalizedName = EditName.Trim();
        var normalizedPhone = EditPhone.Trim();

        var editedContact = new Contact
        {
            Id = _contact.Id,
            Name = normalizedName,
            Phone = normalizedPhone
        };

        if (!editedContact.Validate(out var errorMessage))
        {
            ValidationMessage = errorMessage;
            _dialogService.ShowError(errorMessage);
            return;
        }

        using var context = _contextFactory.CreateDbContext();
        var hasDuplicate = context.Contacts.Any(contact =>
            contact.Id != editedContact.Id && contact.Phone == normalizedPhone);

        if (hasDuplicate)
        {
            ValidationMessage = "Другой контакт уже использует этот номер.";
            _dialogService.ShowWarning(ValidationMessage);
            return;
        }

        var contactToUpdate = context.Contacts.Find(editedContact.Id);
        if (contactToUpdate is null)
        {
            ValidationMessage = "Контакт уже удален или не найден.";
            _dialogService.ShowWarning(ValidationMessage);
            _navigation.NavigateTo<ContactsListViewModel>();
            return;
        }

        contactToUpdate.Name = editedContact.Name;
        contactToUpdate.Phone = editedContact.Phone;
        context.SaveChanges();

        _dialogService.ShowInfo("Изменения сохранены.");
        _navigation.NavigateTo<ContactsListViewModel>();
    }

    private bool CanSave()
    {
        return _contact is not null
            && !string.IsNullOrWhiteSpace(EditName)
            && Contact.IsPhoneValid(EditPhone);
    }

    private void RefreshValidationState()
    {
        if (string.IsNullOrWhiteSpace(EditName))
        {
            ValidationMessage = "Введите имя контакта.";
        }
        else if (string.IsNullOrWhiteSpace(EditPhone))
        {
            ValidationMessage = "Введите номер телефона.";
        }
        else if (!Contact.IsPhoneValid(EditPhone))
        {
            ValidationMessage = "Телефон должен иметь формат +7XXXXXXXXXX или XXXXXXXXXX.";
        }
        else
        {
            ValidationMessage = "Данные корректны. Изменения можно сохранить.";
        }

        CommandManager.InvalidateRequerySuggested();
    }
}
