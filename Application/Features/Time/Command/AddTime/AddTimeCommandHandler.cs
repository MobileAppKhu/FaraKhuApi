using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Time;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Time.Command.AddTime
{
    public class AddTimeCommandHandler : IRequestHandler<AddTimeCommand, AddTimeViewModel>
    {
        private readonly IDatabaseContext _context;
        
        private IStringLocalizer<SharedResource> Localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public AddTimeCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<AddTimeViewModel> Handle(AddTimeCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Instructor user = _context.Instructors.FirstOrDefault(u => u.Id == userId);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var courseObj = _context.Courses.Include(c => c.Times).
                FirstOrDefault(c => c.CourseId == request.CourseId);
            Domain.Models.Time timeObj = new Domain.Models.Time
            {
                Course = courseObj,
                CourseId = courseObj.CourseId,
                StartTime = DateTimeOffset.Parse(request.StartTime).Date,
                EndTime = DateTimeOffset.Parse(request.EndTime).Date
            };
            await _context.Times.AddAsync(timeObj, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new AddTimeViewModel
            {
                Time = _mapper.Map<ViewCourseTimeDto>(timeObj)
            };
        }
    }
}