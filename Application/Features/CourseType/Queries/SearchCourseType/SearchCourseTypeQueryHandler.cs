using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.CourseType;
using AutoMapper;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.CourseType.Queries.SearchCourseType
{
    public class SearchCourseTypeQueryHandler : IRequestHandler<SearchCourseTypeQuery, SearchCourseTypeViewModel>
    {
        public IDatabaseContext _context { get; set; }
        public IMapper _mapper { get; set; }

        public SearchCourseTypeQueryHandler(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SearchCourseTypeViewModel> Handle(SearchCourseTypeQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Domain.Models.CourseType> courseTypeQueryable =
                _context.CourseTypes;

            if (!string.IsNullOrWhiteSpace(request.CourseTypeId))
            {
                courseTypeQueryable =
                    courseTypeQueryable.Where(courseType => courseType.CourseTypeId == request.CourseTypeId);
            }

            if (!string.IsNullOrWhiteSpace(request.CourseTypeTitle))
            {
                courseTypeQueryable = courseTypeQueryable.Where(courseType =>
                    courseType.CourseTypeTitle.Contains(request.CourseTypeTitle));
            }

            if (!string.IsNullOrWhiteSpace(request.CourseTypeCode))
            {
                courseTypeQueryable =
                    courseTypeQueryable.Where(courseType => courseType.CourseTypeCode == request.CourseTypeCode);
            }

            if (!string.IsNullOrWhiteSpace(request.DepartmentId))
            {
                courseTypeQueryable =
                    courseTypeQueryable.Where(courseType => courseType.DepartmentId == request.DepartmentId);
            }

            switch (request.CourseTypeColumn)
            {
                case CourseTypeColumn.CourseTypeId:
                    courseTypeQueryable = request.OrderDirection
                        ? courseTypeQueryable.OrderBy(courseType => courseType.CourseTypeId)
                        : courseTypeQueryable.OrderByDescending(courseType => courseType.CourseTypeId);
                    break;
                case CourseTypeColumn.CourseTypeTitle:
                    courseTypeQueryable = request.OrderDirection
                        ? courseTypeQueryable.OrderBy(courseType => courseType.CourseTypeTitle)
                            .ThenBy(courseType => courseType.CourseTypeId)
                        : courseTypeQueryable.OrderByDescending(courseType => courseType.CourseTypeTitle)
                            .ThenByDescending(courseType => courseType.CourseTypeId);
                    break;
                case CourseTypeColumn.CourseTypeCode:
                    courseTypeQueryable = request.OrderDirection
                        ? courseTypeQueryable.OrderBy(courseType => courseType.CourseTypeCode)
                            .ThenBy(courseType => courseType.CourseTypeId)
                        : courseTypeQueryable.OrderByDescending(courseType => courseType.CourseTypeCode)
                            .ThenByDescending(courseType => courseType.CourseTypeId);
                    break;
                case CourseTypeColumn.DepartmentId:
                    courseTypeQueryable = request.OrderDirection
                        ? courseTypeQueryable.OrderBy(courseType => courseType.DepartmentId)
                            .ThenBy(courseType => courseType.CourseTypeId)
                        : courseTypeQueryable.OrderByDescending(courseType => courseType.DepartmentId)
                            .ThenByDescending(courseType => courseType.CourseTypeId);
                    break;
            }

            int searchCount = await courseTypeQueryable.CountAsync(cancellationToken);

            List<Domain.Models.CourseType> courseTypes = await courseTypeQueryable.Skip(request.Start).Take(request.Step)
                .ToListAsync(cancellationToken);

            return new SearchCourseTypeViewModel
            {
                CourseTypes = _mapper.Map<List<CourseTypeSearchDto>>(courseTypes),
                SearchCount = searchCount
            };
        }
    }
}