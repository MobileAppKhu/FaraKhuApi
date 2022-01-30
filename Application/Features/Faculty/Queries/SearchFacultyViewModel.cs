using System.Collections.Generic;
using Application.DTOs.Faculty;

namespace Application.Features.Faculty.Queries
{
    public class SearchFacultyViewModel
    {
        public List<FacultySearchDto> Faculties { get; set; }
        public int SearchCount { get; set; }
    }
}