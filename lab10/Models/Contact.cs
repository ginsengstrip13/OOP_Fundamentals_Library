using System.Text.RegularExpressions;
using lab10.ViewModels;

namespace lab10.Models;

/// <summary>
/// Model в паттерне MVVM.
/// Contact хранит данные телефонной книги и проверяет правила предметной области.
/// </summary>
public sealed partial class Contact : ObservableObject
{
    private string _name = string.Empty;
    private string _phone = string.Empty;

    /// <summary>
    /// Создает контакт только из корректных данных.
    /// Проверка остается в Model, чтобы бизнес-правила не зависели от интерфейса окна.
    /// </summary>
    public Contact(string name, string phone)
    {
        Name = name.Trim();
        Phone = phone.Trim();

        if (!Validate(out var errorMessage))
        {
            throw new ArgumentException(errorMessage);
        }
    }

    /// <summary>
    /// Имя контакта. При изменении уведомляет привязки WPF.
    /// </summary>
    public string Name
    {
        get => _name;
        set => Set(ref _name, value.Trim());
    }

    /// <summary>
    /// Телефон контакта. Допустимые форматы: +7XXXXXXXXXX или XXXXXXXXXX.
    /// </summary>
    public string Phone
    {
        get => _phone;
        set => Set(ref _phone, value.Trim());
    }

    /// <summary>
    /// Проверяет корректность контакта и возвращает текст ошибки для ViewModel.
    /// </summary>
    public bool Validate(out string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            errorMessage = "Имя контакта не должно быть пустым.";
            return false;
        }

        if (!IsPhoneValid(Phone))
        {
            errorMessage = "Телефон должен иметь формат +7XXXXXXXXXX или XXXXXXXXXX.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    /// <summary>
    /// Отдельная проверка телефона нужна ViewModel до создания объекта Contact.
    /// </summary>
    public static bool IsPhoneValid(string phone)
    {
        return PhoneRegex().IsMatch(phone.Trim());
    }

    /// <summary>
    /// Регулярное выражение принимает российский номер с кодом +7 и 10 цифрами
    /// либо локальный вариант без кода страны из 10 цифр.
    /// </summary>
    [GeneratedRegex(@"^(?:\+7\d{10}|\d{10})$")]
    private static partial Regex PhoneRegex();
}
