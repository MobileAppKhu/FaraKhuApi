using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Course;
using Application.DTOs.Time;
using Application.Features.Notification.SystemCallCommands;
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

namespace Application.Features.Course.Commands.AddCourse
{
    public class AddCourseCommandHandler : IRequestHandler<AddCourseCommand, AddCourseViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper Mapper { get; }

        public AddCourseCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            Mapper = mapper;
        }

        public async Task<AddCourseViewModel> Handle(AddCourseCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            if (user.UserType != UserType.Owner && user.UserType != UserType.Instructor)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            if (!string.IsNullOrWhiteSpace(request.InstructorId))
            {
                if (user.UserType != UserType.Owner)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.Unauthorized,
                        Message = Localizer["Unauthorized"]
                    });
                }

                user = await 
                    _context.Instructors.FirstOrDefaultAsync(
                        instructor => instructor.InstructorId == request.InstructorId, cancellationToken);
                if (user == null)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.InstructorNotFound,
                        Message = Localizer["InstructorNotFound"]
                    });
                }
            }

            Domain.Models.CourseType courseType = await _context.CourseTypes
                .Include(type => type.Department)
                .ThenInclude(department => department.Faculty)
                .FirstOrDefaultAsync(type => type.CourseTypeId == request.CourseTypeId,
                    cancellationToken);
            if (courseType == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CourseTypeNotFound,
                    Message = Localizer["CourseTypeNotFound"]
                });
            }

            Domain.Models.Course courseObj = new Domain.Models.Course
            {
                CourseTypeId = request.CourseTypeId,
                Address = request.Address,
                CourseType = courseType,
                Instructor = (Instructor)user,
                InstructorId = user.Id,
                EndDate = request.EndDate
            };
            await _context.Courses.AddAsync(courseObj, cancellationToken);

            List<Student> students =
                _context.Students.Include(student => student.Courses)
                    .Where(student => request.AddStudentDto.StudentIds.Contains(student.StudentId)).ToList();
            if (students.Count != request.AddStudentDto.StudentIds.Count)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.StudentNotFound,
                    Message = Localizer["StudentNotFound"]
                });
            }

            foreach (var student in students)
            {
                student.Courses.Add(courseObj);
                NotificationAdder.AddNotification(_context,
                    Localizer["YouHaveBeenAddedToACourse"],
                    courseObj.CourseId, NotificationObjectType.Course, NotificationOperation.AddStudentToCourse,
                    student);
            }

            foreach (var time in request.AddTimeDtos)
            {
                string[] startTimes = time.StartTime.Split("-");
                string[] endTime = time.EndTime.Split("-");
                await _context.Times.AddAsync(new Domain.Models.Time
                {
                    Course = courseObj,
                    CourseId = courseObj.CourseId,
                    StartTime = new DateTime(2000, 12, 25, Int32.Parse(startTimes[0]), Int32.Parse(startTimes[1]), 0),
                    EndTime = new DateTime(2000, 12, 25, Int32.Parse(endTime[0]), Int32.Parse(endTime[1]), 0),
                    WeekDay = time.WeekDay
                }, cancellationToken);
            }

            foreach (var timei in courseObj.Times)
            {
                foreach (var timej in courseObj.Times)
                {
                    if (timei.Equals(timej))
                    {
                        continue;
                    }

                    if ((timei.WeekDay == timej.WeekDay) &&
                        ((timei.StartTime < timej.StartTime && timei.EndTime > timej.StartTime) ||
                         (timei.EndTime > timej.EndTime && timei.StartTime < timej.EndTime)))
                    {
                        throw new CustomException(new Error
                        {
                            ErrorType = ErrorType.TimeConflict,
                            Message = Localizer["TimeConflict"]
                        });
                    }
                }
            }

            var avatarObj =
                await _context.Files.FirstOrDefaultAsync(avatar => avatar.Id == request.AvatarId, cancellationToken);
            if (avatarObj == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.FileNotFound,
                    Message = Localizer["FileNotFound"]
                });
            }

            courseObj.Avatar = avatarObj;
            courseObj.AvatarId = avatarObj.Id;

            await _context.SaveChangesAsync(cancellationToken);
            return new AddCourseViewModel
            {
                Course = Mapper.Map<SearchCourseDto>(courseObj)
            };
        }
    }
}