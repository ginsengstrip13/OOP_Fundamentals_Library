using System.Windows.Input;
using lab12.Models;
using lab12.Services;

namespace lab12.ViewModels;

/// <summary>
/// ViewModel экрана редактирования принимает выбранный Contact через INavigationAware.
/// </summary>
public sealed class ContactEditViewModel : ObservableObject, INavigationAware
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigation;
    private readonly IContactsRepository _contactsRepository;
    private Contact? _contact;
    private string _editName = string.Empty;
    private string _editPhone = string.Empty;
    private string _validationMessage = "Измените данные контакта и нажмите Сохранить.";

    public ContactEditViewModel(
        IDialogService dialogService,
        INavigationService navigation,
        IContactsRepository contactsRepository)
    {
        // Экран редактирования использует те же сервисы, что и список:
        // диалоги для сообщений, навигацию для возврата, репозиторий для проверки дублей.
        _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        _navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        _contactsRepository = contactsRepository ?? throw new ArgumentNullException(nameof(contactsRepository));

        // SaveCommand сохраняет изменения и возвращает пользователя к списку контактов.
        // CancelCommand ничего не меняет и тоже выполняет навигацию назад.
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
        // NavigationService передает сюда параметр из NavigateTo<ContactEditViewModel>(contact).
        // Если параметр не Contact, экран не сможет редактировать запись и покажет сообщение.
        if (parameter is not Contact contact)
        {
            ValidationMessage = "Контакт для редактирования не выбран.";
            return;
        }

        _contact = contact;

        // Копируем значения в отдельные свойства формы.
        // Модель изменится только после нажатия "Сохранить".
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
        // При редактировании можно оставить свой старый номер,
        // но нельзя взять номер, который принадлежит другому контакту.
        var hasDuplicate = _contactsRepository.Contacts.Any(contact =>
            !ReferenceEquals(contact, _contact)
            && contact.Phone.Equals(normalizedPhone, StringComparison.OrdinalIgnoreCase));

        if (hasDuplicate)
        {
            ValidationMessage = "Другой контакт уже использует этот номер.";
            _dialogService.ShowWarning(ValidationMessage);
            return;
        }

        // Сущность Contact отслеживается DbContext, поэтому SaveChanges сохранит новые значения в SQLite.
        _contact.Name = normalizedName;
        _contact.Phone = normalizedPhone;

        if (!_contact.Validate(out var errorMessage))
        {
            ValidationMessage = errorMessage;
            _dialogService.ShowError(errorMessage);
            return;
        }

        _contactsRepository.SaveChanges();
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
        // Валидация здесь нужна для подсказки пользователю и для доступности кнопки "Сохранить".
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
