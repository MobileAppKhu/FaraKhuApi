using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Student;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.SearchStudent
{
    public class SearchStudentQueryHandler : IRequestHandler<SearchStudentQuery, SearchStudentViewModel>
    {
        private readonly IMapper _mapper;
        public IStringLocalizer<SharedResource> Localizer { get; }
        private readonly IDatabaseContext _context;

        public SearchStudentQueryHandler(IMapper mapper, IStringLocalizer<SharedResource> localizer
            , IDatabaseContext context)
        {
            _mapper = mapper;
            Localizer = localizer;
            _context = context;
        }

        public async Task<SearchStudentViewModel> Handle(SearchStudentQuery request,
            CancellationToken cancellationToken)
        {
            Student student =
                await _context.Students.FirstOrDefaultAsync(st => st.StudentId == request.StudentId, cancellationToken);
            if (student == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.StudentNotFound,
                    Message = Localizer["StudentNotFound"]
                });
            }

            return new SearchStudentViewModel
            {
                StudentShortDto = _mapper.Map<StudentShortDto>(student)
            };
        }
    }
}