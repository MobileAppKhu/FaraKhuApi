using System.Text.RegularExpressions;

namespace Application.Utilities;

public static class EmailValidator
{
    public static bool IsEmail(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        return new Regex(
            @"^[A-Za-z0-9!'#$%&*+\/=?^_`{|}~-]+(?:\.[A-Za-z0-9!'#$%&*+\/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\.)+[A-Za-z]{2,}$",
            RegexOptions.IgnoreCase).IsMatch(text);
    }
}