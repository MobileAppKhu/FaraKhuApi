using System.Threading.Tasks;
using Application.Utilities;
using Xunit;

namespace UnitTest.UtilitiesTest
{
    public class UtilitiesTest
    {
        [Fact]
        public void ConfirmationEmailCodeGenerator_ShouldWorkCorrectly()
        {
            var response = ConfirmEmailCodeGenerator.GenerateCode();
            Assert.Equal(typeof(string), response.GetType());
        }
        
        [Fact]
        public void EmailValidator_ShouldWorkCorrectly()
        {
            var email = "is@correct.email";
            Assert.True(email.IsEmail());
        }
        
        [Fact]
        public void EmailValidator_ShouldNotWorkCorrectly()
        {
            var email = "isNotACorrect.email";
            Assert.True(!email.IsEmail());
        }
    }
}