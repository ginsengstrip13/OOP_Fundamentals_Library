using System.Text.RegularExpressions;
using lab11.ViewModels;

namespace lab11.Models;

/// <summary>
/// Model хранит данные телефонной книги и проверяет правила предметной области.
/// </summary>
public sealed partial class Contact : ObservableObject
{
    // Поля закрыты от внешнего кода: изменение идет через свойства,
    // чтобы при изменении сработал Set(...) и WPF получил уведомление PropertyChanged.
    private string _name = string.Empty;
    private string _phone = string.Empty;

    public Contact(string name, string phone)
    {
        // В конструкторе данные сразу проходят через свойства,
        // чтобы применились Trim() и единые правила записи.
        Name = name;
        Phone = phone;

        // Model сама проверяет предметные правила.
        // Это не обязанность View и не обязанность ViewModel.
        if (!Validate(out var errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }
    }

    public string Name
    {
        get => _name;
        set => Set(ref _name, (value ?? string.Empty).Trim());
    }

    public string Phone
    {
        get => _phone;
        set => Set(ref _phone, (value ?? string.Empty).Trim());
    }

    public bool Validate(out string errorMessage)
    {
        // Имя обязательно: пустой контакт в телефонной книге не имеет смысла.
        if (string.IsNullOrWhiteSpace(Name))
        {
            errorMessage = "Имя контакта не должно быть пустым.";
            return false;
        }

        // Формат телефона является правилом модели, поэтому проверка находится здесь.
        if (!IsPhoneValid(Phone))
        {
            errorMessage = "Телефон должен иметь формат +7XXXXXXXXXX или XXXXXXXXXX.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    public static bool IsPhoneValid(string? phone)
    {
        // Метод static, чтобы ViewModel могла заранее проверить ввод
        // до попытки создать новый объект Contact.
        return !string.IsNullOrWhiteSpace(phone) && PhoneRegex().IsMatch(phone.Trim());
    }

    // Регулярное выражение принимает российский номер с +7 и 10 цифрами
    // либо локальный вариант из 10 цифр без кода страны.
    [GeneratedRegex(@"^(?:\+7\d{10}|\d{10})$")]
    private static partial Regex PhoneRegex();
}
