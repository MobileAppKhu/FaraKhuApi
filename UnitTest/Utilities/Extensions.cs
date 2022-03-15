using System;
using System.Globalization;
using System.Linq;
using System.Text;

using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Application.Features.Account.Commands.SignIn;
using Domain.Enum;


namespace UnitTest.Utilities
{
    public static class Extensions
    {
        public static async Task<bool> HasErrorCode(this HttpResponseMessage response, ErrorType? errorCode = null)
        {
            var errors = (await response.GetJson())["errors"];
            if (errorCode == null)
                return errors != null && errors.ToArray().Any();
            return errors != null && errors.ToArray().Any(error => (ErrorType) error["code"].Value<int>() == errorCode);
        }

        private static async Task<JObject> GetJson(this HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(content) ? new JObject() : JObject.Parse(content);
        }

        public static async Task<HttpResponseMessage> PostAsync<T>(this HttpClient client, string url, T data)
        {
            return await client.PostAsync(url, JsonContent.Create(data));
        }

        public static async Task<string> GetContent(this HttpResponseMessage message)
        {
            var content = Regex.Replace(
                await message.Content.ReadAsStringAsync(),
                @"\\u(?<Value>[a-fA-F0-9]{4})",
                m => ((char) int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());

            try
            {
                if (!content.StartsWith("{"))
                    throw new Exception();

                var indentation = 0;
                var quoteCount = 0;
                const string indentString = "    ";
                var result =
                    from ch in content
                    let quotes = ch == '"' ? quoteCount++ : quoteCount
                    let lineBreak = ch == ',' && quotes % 2 == 0
                        ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indentString, indentation))
                        : null
                    let openChar = ch == '{' || ch == '['
                        ? ch + Environment.NewLine + string.Concat(Enumerable.Repeat(indentString, ++indentation))
                        : ch.ToString()
                    let closeChar = ch == '}' || ch == ']'
                        ? Environment.NewLine + string.Concat(Enumerable.Repeat(indentString, --indentation)) + ch
                        : ch.ToString()
                    select lineBreak ?? (openChar.Length > 1
                        ? openChar
                        : closeChar);

                return string.Concat(result);
            }
            catch
            {
                return content;
            }
        }

        public static async Task AuthToStudent(this HttpClient client)
        {
            var userObj = new SignInCommand
            {
                Logon = "Student@Farakhu.app",
                Password = "StudentPassword"
            };
            var user = JsonConvert.SerializeObject(userObj);

            var response = await client.PostAsync( "api/Account/SignIn",
                new StringContent(user, Encoding.UTF8, "application/json"));

            client.DefaultRequestHeaders.Add("Cookie", response.Headers.GetValues("Set-Cookie").ToArray()[0]);
        }
        
        public static async Task AuthToSecondStudent(this HttpClient client)
        {
            var userObj = new SignInCommand
            {
                Logon = "SecondStudent@Farakhu.app",
                Password = "SecondStudentPassword"
            };
            var user = JsonConvert.SerializeObject(userObj);

            var response = await client.PostAsync( "api/Account/SignIn",
                new StringContent(user, Encoding.UTF8, "application/json"));

            client.DefaultRequestHeaders.Add("Cookie", response.Headers.GetValues("Set-Cookie").ToArray()[0]);
        }

        public static async Task AuthToInstructor(this HttpClient client)
        {
            var userObj = new SignInCommand
            {
                Logon = "Instructor@Farakhu.app",
                Password = "InstructorPassword"
            };
            var user = JsonConvert.SerializeObject(userObj);

            var response = await client.PostAsync("api/Account/SignIn",
                new StringContent(user, Encoding.UTF8, "application/json"));

            client.DefaultRequestHeaders.Add("Cookie", response.Headers.GetValues("Set-Cookie").ToArray()[0]);
        }
        
        public static async Task AuthToSecondInstructor(this HttpClient client)
        {
            var userObj = new SignInCommand
            {
                Logon = "SecondInstructor@Farakhu.app",
                Password = "SecondInstructorPassword"
            };
            var user = JsonConvert.SerializeObject(userObj);

            var response = await client.PostAsync("api/Account/SignIn",
                new StringContent(user, Encoding.UTF8, "application/json"));

            client.DefaultRequestHeaders.Add("Cookie", response.Headers.GetValues("Set-Cookie").ToArray()[0]);
        }
        
        public static async Task AuthToOwner(this HttpClient client)
        {
            var userObj = new SignInCommand
            {
                Logon = "Owner@FaraKhu.app",
                Password = "OwnerPassword"
            };
            var user = JsonConvert.SerializeObject(userObj);

            var response = await client.PostAsync("api/Account/SignIn",
                new StringContent(user, Encoding.UTF8, "application/json"));

            client.DefaultRequestHeaders.Add("Cookie", response.Headers.GetValues("Set-Cookie").ToArray()[0]);
        }
    }
}