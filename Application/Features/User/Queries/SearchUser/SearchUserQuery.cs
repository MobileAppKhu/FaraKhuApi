using MediatR;

namespace Application.Features.User.Queries.SearchUser
{
    public class SearchUserQuery : IRequest<SearchUserViewModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LinkedIn { get; set; }
        public string GoogleScholar { get; set; }
        public string StudentId { get; set; }
        public string InstructorId { get; set; }
    }
}