using System;
using System.Text;

namespace Application.Utilities
{
    public class ConfirmEmailCodeGenerator
    {
        public static string GenerateCode()
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 5; ++i)
            {
                sb.Append((char)(random.Next(0,26) + 65));
            }
            return sb.ToString();
        }
    }
}