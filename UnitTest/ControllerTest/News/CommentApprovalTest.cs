using UnitTest.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.ControllerTest.News
{
    public class CommentApprovalTest : AppFactory
    {
        private readonly string _path = "/api/News/CommentApproval";
        private readonly ITestOutputHelper _outputHelper;

        public CommentApprovalTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
    }
}