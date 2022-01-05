using Domain.Enum;
using MediatR;

namespace Application.Features.Faculty.Queries
{
    public class SearchFacultyQuery : IRequest<SearchFacultyViewModel>
    {
        public string FacultyId { get; set; }
        public string FacultyTitle { get; set; }
        public string FacultyCode { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
        public FacultyColumn FacultyColumn { get; set; }
        public bool OrderDirection { get; set; }
    }
}