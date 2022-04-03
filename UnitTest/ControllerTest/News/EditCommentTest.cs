using UnitTest.Utilities;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.News
{
    public class EditCommentTest : AppFactory
    {
        
        private readonly ITestOutputHelper _outputHelper;
        private readonly string _path = "api/News/EditComment";

        public EditCommentTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        
    }
}