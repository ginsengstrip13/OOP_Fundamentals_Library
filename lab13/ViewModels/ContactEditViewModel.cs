using System.Windows.Input;
using lab13.Data;
using lab13.Models;
using lab13.Services;
using Microsoft.EntityFrameworkCore;

namespace lab13.ViewModels;

public sealed class ContactEditViewModel : ObservableObject, INavigationAware
{
    private readonly ApplicationContext _context;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigation;
    private Contact? _contact;
    private string _editName = string.Empty;
    private string _editPhone = string.Empty;
    private string _validationMessage = "Измените данные контакта и нажмите Сохранить.";

    public ContactEditViewModel(
        ApplicationContext context,
        IDialogService dialogService,
        INavigationService navigation)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
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
        var hasDuplicate = _context.Contacts.Any(contact =>
            contact.Id != _contact.Id && contact.Phone == normalizedPhone);

        if (hasDuplicate)
        {
            ValidationMessage = "Другой контакт уже использует этот номер.";
            _dialogService.ShowWarning(ValidationMessage);
            return;
        }

        _contact.Name = normalizedName;
        _contact.Phone = normalizedPhone;

        if (!_contact.Validate(out var errorMessage))
        {
            ValidationMessage = errorMessage;
            _dialogService.ShowError(errorMessage);
            return;
        }

        _context.Entry(_contact).State = EntityState.Modified;
        _context.SaveChanges();

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
