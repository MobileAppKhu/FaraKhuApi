using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Department;
using AutoMapper;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Department.Queries.SearchDepartment
{
    public class SearchDepartmentQueryHandler : IRequestHandler<SearchDepartmentQuery, SearchDepartmentViewModel>
    {
        public IDatabaseContext _context { get; set; }
        public IMapper _mapper { get; set; }

        public SearchDepartmentQueryHandler(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SearchDepartmentViewModel> Handle(SearchDepartmentQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Domain.Models.Department> departmentQueryable =
                _context.Departments;

            if (!string.IsNullOrWhiteSpace(request.DepartmentId))
            {
                departmentQueryable =
                    departmentQueryable.Where(department => department.DepartmentId == request.DepartmentId);
            }

            if (!string.IsNullOrWhiteSpace(request.DepartmentTitle))
            {
                departmentQueryable = departmentQueryable.Where(department =>
                    department.DepartmentTitle.Contains(request.DepartmentTitle));
            }

            if (!string.IsNullOrWhiteSpace(request.DepartmentCode))
            {
                departmentQueryable =
                    departmentQueryable.Where(department => department.DepartmentCode == request.DepartmentCode);
            }

            if (!string.IsNullOrWhiteSpace(request.FacultyId))
            {
                departmentQueryable =
                    departmentQueryable.Where(department => department.FacultyId == request.FacultyId);
            }

            switch (request.DepartmentColumn)
            {
                case DepartmentColumn.DepartmentId:
                    departmentQueryable = request.OrderDirection
                        ? departmentQueryable.OrderBy(department => department.DepartmentId)
                        : departmentQueryable.OrderByDescending(department => department.DepartmentId);
                    break;
                case DepartmentColumn.DepartmentTitle:
                    departmentQueryable = request.OrderDirection
                        ? departmentQueryable.OrderBy(department => department.DepartmentTitle)
                            .ThenBy(department => department.DepartmentId)
                        : departmentQueryable.OrderByDescending(department => department.DepartmentTitle)
                            .ThenByDescending(department => department.DepartmentId);
                    break;
                case DepartmentColumn.DepartmentCode:
                    departmentQueryable = request.OrderDirection
                        ? departmentQueryable.OrderBy(department => department.DepartmentCode)
                            .ThenBy(department => department.DepartmentId)
                        : departmentQueryable.OrderByDescending(department => department.DepartmentCode)
                            .ThenByDescending(department => department.DepartmentId);
                    break;
                case DepartmentColumn.FacultyId:
                    departmentQueryable = request.OrderDirection
                        ? departmentQueryable.OrderBy(department => department.FacultyId)
                            .ThenBy(department => department.DepartmentId)
                        : departmentQueryable.OrderByDescending(department => department.FacultyId)
                            .ThenByDescending(department => department.DepartmentId);
                    break;
            }

            int searchCount = await departmentQueryable.CountAsync(cancellationToken);

            List<Domain.Models.Department> departments = await departmentQueryable.Skip(request.Start).Take(request.Step)
                .ToListAsync(cancellationToken);

            return new SearchDepartmentViewModel
            {
                Departments = _mapper.Map<List<DepartmentSearchDto>>(departments),
                SearchCount = searchCount
            };
        }
    }
}