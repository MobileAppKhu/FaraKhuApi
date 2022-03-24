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

            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                editingCourse.Address = request.Address;
            }

            if (request.EndDate != null)
            {
                editingCourse.EndDate = request.EndDate.Value;
            }

            if (request.AvatarId != null)
            {
                var avatar =
                    await _context.Files.FirstOrDefaultAsync(avatar => avatar.Id == request.AvatarId,
                        cancellationToken);
                if (avatar == null)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.FileNotFound,
                        Message = Localizer["FileNotFound"]
                    });
                }

                editingCourse.AvatarId = request.AvatarId;
                editingCourse.Avatar = avatar;
            }

            if (request.AddStudentDto != null && request.AddStudentDto.StudentIds.Count != 0)
            {
                List<Student> addStudents = _context.Students
                    .Include(student => student.Courses)
                    .Where(student => request.AddStudentDto.StudentIds.Contains(student.StudentId) &&
                                      !student.Courses.Contains(editingCourse)).ToList();
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
                if (deleteStudents.Count != request.DeleteStudentDto.StudentIds.Count)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.StudentNotFound,
                        Message = Localizer["StudentNotFound"]
                    });
                }
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
                var times = await _context.Times.Where(time => request.DeleteTimeDto.TimeIds.Contains(time.TimeId))
                    .ToListAsync(cancellationToken);
                if (times.Count != request.DeleteTimeDto.TimeIds.Count)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.TimeNotFound,
                        Message = Localizer["TimeNotFound"]
                    });
                }
                foreach (var time in times)
                {
                    if (!editingCourse.Times.Contains(time))
                    {
                        throw new CustomException(new Error
                        {
                            ErrorType = ErrorType.TimeNotFound,
                            Message = Localizer["TimeNotFound"]
                        });
                    }
                    
                    editingCourse.Times.Remove(time);
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