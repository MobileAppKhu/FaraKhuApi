using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Faculty;
using Application.Resources;
using AutoMapper;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Faculty.Queries
{
    public class SearchFacultyQueryHandler : IRequestHandler<SearchFacultyQuery, SearchFacultyViewModel>
    {
        public IDatabaseContext _context { get; set; }
        public IMapper _mapper { get; set; }

        public SearchFacultyQueryHandler(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<SearchFacultyViewModel> Handle(SearchFacultyQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Domain.Models.Faculty> facultyQueryable = _context.Faculties;

            if (!string.IsNullOrWhiteSpace(request.FacultyId))
            {
                facultyQueryable = facultyQueryable.Where(faculty => faculty.FacultyId == request.FacultyId);
            }

            if (!string.IsNullOrWhiteSpace(request.FacultyTitle))
            {
                facultyQueryable = facultyQueryable.Where(faculty => faculty.FacultyTitle.Contains(request.FacultyTitle));
            }

            if (!string.IsNullOrWhiteSpace(request.FacultyCode))
            {
                facultyQueryable = facultyQueryable.Where(faculty => faculty.FacultyCode == request.FacultyCode);
            }

            switch (request.FacultyColumn)
            {
                case FacultyColumn.FacultyId:
                    facultyQueryable = request.OrderDirection
                        ? facultyQueryable.OrderBy(faculty => faculty.FacultyId)
                        : facultyQueryable.OrderByDescending(faculty => faculty.FacultyId);
                    break;
                case FacultyColumn.FacultyTitle:
                    facultyQueryable = request.OrderDirection
                        ? facultyQueryable.OrderBy(faculty => faculty.FacultyTitle).ThenBy(faculty => faculty.FacultyId)
                        : facultyQueryable.OrderByDescending(faculty => faculty.FacultyTitle)
                            .ThenByDescending(faculty => faculty.FacultyId);
                    break;
                case FacultyColumn.FacultyCode:
                    facultyQueryable = request.OrderDirection
                        ? facultyQueryable.OrderBy(faculty => faculty.FacultyCode).ThenBy(faculty => faculty.FacultyId)
                        : facultyQueryable.OrderByDescending(faculty => faculty.FacultyCode)
                            .ThenByDescending(faculty => faculty.FacultyId);
                    break;
            }

            int searchCount = await facultyQueryable.CountAsync(cancellationToken);

            List<Domain.Models.Faculty> faculties = await facultyQueryable.Skip(request.Start).Take(request.Step)
                .ToListAsync(cancellationToken);
            
            return new SearchFacultyViewModel
            {
                Faculties = _mapper.Map<List<FacultySearchDto>>(faculties),
                SearchCount = searchCount
            };
        }
    }
}