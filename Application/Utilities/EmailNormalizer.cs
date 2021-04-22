using System.Text.RegularExpressions;

namespace Application.Utilities
{
    public static class EmailNormalizer
    {
        public static string EmailNormalize(this string email)
        {
            if (email == null)
                return null;

            if (!new Regex(@"@.+\..+$", RegexOptions.IgnoreCase).IsMatch(email))
                return email;

            var emailFirstPart = email.Split("@")[0].Replace(".", "");
            var emailSecondPart = email.Split("@")[1];
            return emailFirstPart + "@" + emailSecondPart;
        }
    }
}