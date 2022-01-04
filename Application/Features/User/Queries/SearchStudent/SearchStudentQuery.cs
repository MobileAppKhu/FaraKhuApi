using MediatR;

namespace Application.Features.User.Queries.SearchStudent
{
    public class SearchStudentQuery : IRequest<SearchStudentViewModel>
    {
        public string StudentId { get; set; }
    }
}