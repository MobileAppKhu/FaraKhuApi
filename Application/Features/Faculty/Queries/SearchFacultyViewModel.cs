using System.Collections.Generic;

namespace Application.Features.Faculty.Queries
{
    public class SearchFacultyViewModel
    {
        public List<Domain.Models.Faculty> Faculties { get; set; }
        public int SearchCount { get; set; }
    }
}