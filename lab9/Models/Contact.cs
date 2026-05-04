using System.Text.RegularExpressions;
using lab9.ViewModels;

namespace lab9.Models;

/// <summary>
/// Model в паттерне MVVM.
/// Класс Contact хранит бизнес-данные телефонной книги: имя контакта и номер телефона.
/// Модель не знает о TextBox, DataGrid, кнопках и других элементах View.
/// </summary>
public sealed partial class Contact : ObservableObject
{
    private string _name = string.Empty;
    private string _phone = string.Empty;

    /// <summary>
    /// Конструктор получает начальные значения и проверяет бизнес-правила модели.
    /// Если имя или телефон некорректны, объект Contact не создается.
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
    /// Имя контакта. Set уведомляет привязки WPF, если значение изменилось.
    /// </summary>
    public string Name
    {
        get => _name;
        set => Set(ref _name, value.Trim());
    }

    /// <summary>
    /// Номер телефона контакта. Допустимые форматы: +7XXXXXXXXXX или XXXXXXXXXX.
    /// </summary>
    public string Phone
    {
        get => _phone;
        set => Set(ref _phone, value.Trim());
    }

    /// <summary>
    /// Проверяет контакт по правилам предметной области.
    /// Этот метод находится в Model, потому что формат телефона является правилом данных,
    /// а не особенностью внешнего вида окна.
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
    /// Отдельный метод нужен ViewModel для предварительной проверки введенного телефона
    /// до создания объекта Contact и добавления его в ObservableCollection.
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

