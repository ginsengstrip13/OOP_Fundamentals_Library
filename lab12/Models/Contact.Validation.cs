using System.Text.RegularExpressions;

namespace lab12.Models;

public partial class Contact
{
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

    public static bool IsPhoneValid(string? phone)
    {
        return !string.IsNullOrWhiteSpace(phone) && PhoneRegex().IsMatch(phone.Trim());
    }

    [GeneratedRegex(@"^(?:\+7\d{10}|\d{10})$")]
    private static partial Regex PhoneRegex();
}
