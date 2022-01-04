using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Notification.SystemCallCommands;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Commands.EditCourse
{
    public class EditCourseCommandHandler : IRequestHandler<EditCourseCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public EditCourseCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(EditCourseCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Instructor user = _context.Instructors.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var editingCourse =
                await _context.Courses.Include(course => course.Students)
                    .Include(course => course.Times)
                    .Include(course => course.CourseEvents)
                    .Include(course => course.Instructor)
                    .Include(course => course.Polls)
                    .FirstOrDefaultAsync(course => course.CourseId == request.CourseId,
                        cancellationToken: cancellationToken);

            if (editingCourse == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.CourseNotFound,
                    Message = Localizer["CourseNotFound"]
                });
            }

            if (editingCourse.Instructor != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            if (!string.IsNullOrWhiteSpace(request.CourseTypeId))
            {
                var courseType = await
                    _context.CourseTypes.FirstOrDefaultAsync(type => type.CourseTypeId == request.CourseTypeId,
                        cancellationToken);
                if (courseType == null)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.CourseTypeNotFound,
                        Message = Localizer["CourseTypeNotFound"]
                    });
                }

                editingCourse.CourseType = courseType;
                editingCourse.CourseTypeId = request.CourseTypeId;

            }

            if (request.AddStudentDto != null && request.AddStudentDto.StudentIds.Count != 0)
            {
                List<Student> addStudents = _context.Students
                    .Where(student => request.AddStudentDto.StudentIds.Contains(student.StudentId)).ToList();
                if (addStudents.Count != request.AddStudentDto.StudentIds.Count)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.StudentNotFound,
                        Message = Localizer["StudentNotFound"]
                    });
                }

                foreach (var student in addStudents)
                {
                    editingCourse.Students.Add(student);
                    NotificationAdder.AddNotification(_context,
                        Localizer["YouHaveBeenAddedToACourse"],
                        editingCourse.CourseId, NotificationObjectType.Course, NotificationOperation.AddStudentToCourse,
                        student);
                }
            }

            if (request.DeleteStudentDto != null && request.DeleteStudentDto.StudentIds.Count != 0)
            {
                List<Student> deleteStudents = await _context.Students
                    .Where(student => request.DeleteStudentDto.StudentIds.Contains(student.StudentId)).ToListAsync(cancellationToken);

                foreach (var student in deleteStudents)
                {
                    if (!editingCourse.Students.Contains(student))
                    {
                        throw new CustomException(new Error
                        {
                            ErrorType = ErrorType.StudentNotFound,
                            Message = Localizer["StudentNotFound"]
                        });
                    }

                    editingCourse.Students.Remove(student);
                }
            }
            
            // delete time
            if (request.DeleteTimeDto != null && request.DeleteTimeDto.TimeIds.Count != 0)
            {
                foreach (var time in editingCourse.Times)
                {
                    if (request.DeleteTimeDto.TimeIds.Contains(time.TimeId))
                    {
                        editingCourse.Times.Remove(time);
                    }
                    else
                    {
                        throw new CustomException(new Error
                        {
                            ErrorType = ErrorType.TimeNotFound,
                            Message = Localizer["TimeNotFound"]
                        });
                    }
                }
            }

            // add time
            if (request.AddTimeDtos != null && request.AddTimeDtos.Count != 0)
            {
                foreach (var addTimeDto in request.AddTimeDtos)
                {
                    string[] startTime = addTimeDto.StartTime.Split("-");
                    string[] endTime = addTimeDto.EndTime.Split("-");
                    editingCourse.Times.Add(new Domain.Models.Time
                    {
                        Course = editingCourse,
                        CourseId = editingCourse.CourseId,
                        WeekDay = addTimeDto.WeekDay,
                        StartTime = new DateTime(2000, 12, 25, Int32.Parse(startTime[0]),
                            Int32.Parse(startTime[1]), 0),
                        EndTime = new DateTime(2000, 12, 25, Int32.Parse(endTime[0]),
                            Int32.Parse(endTime[1]), 0)
                    });
                }
            }

            foreach (var timei in editingCourse.Times)
            {
                foreach (var timej in editingCourse.Times)
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

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}