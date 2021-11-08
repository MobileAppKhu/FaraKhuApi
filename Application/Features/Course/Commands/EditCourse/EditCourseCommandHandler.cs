﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Identity;
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
                await _context.Courses.Include(course => course.Students).Include(course => course.Times)
                    .Include(course => course.CourseEvents)
                    .Include(course => course.Instructor)
                    .Include(course => course.Polls)
                    .FirstOrDefaultAsync(course => course.CourseId == request.CourseId,
                        cancellationToken: cancellationToken);

            if (editingCourse == null)
            {
                // return error
                return Unit.Value;
            }

            if (request.AddStudentDto != null && request.AddStudentDto.StudentIds.Count != 0)
            {
                List<Student> addStudents = _context.Students
                    .Where(student => request.AddStudentDto.StudentIds.Contains(student.StudentId)).ToList();
                if (addStudents.Count != request.AddStudentDto.StudentIds.Count)
                {
                    // return error
                }

                foreach (var student in addStudents)
                {
                    editingCourse.Students.Add(student);
                }
            }

            if (request.DeleteStudentDto != null && request.DeleteStudentDto.StudentIds.Count != 0)
            {
                List<Student> deleteStudents = _context.Students
                    .Where(student => request.DeleteStudentDto.StudentIds.Contains(student.StudentId)).ToList();
                if (deleteStudents.Count != request.DeleteStudentDto.StudentIds.Count)
                {
                    // return error
                }

                foreach (var student in deleteStudents)
                {
                    if (!editingCourse.Students.Contains(student))
                    {
                        // return error
                    }

                    editingCourse.Students.Remove(student);
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
            // edit time
            if (request.EditTimeDtos.Count != 0)
            {
                foreach (var editTimeDto in request.EditTimeDtos)
                {
                    if (editTimeDto.TimeId == null)
                    {
                        // return error
                    }
                    var time = editingCourse.Times.First(t => t.TimeId == editTimeDto.TimeId);
                    if (time == null)
                    {
                        // return error
                    }

                    if (!String.IsNullOrEmpty(editTimeDto.StartTime))
                    {
                        string[] startTime = editTimeDto.StartTime.Split("-");
                        editingCourse.Times.FirstOrDefault(t => t.TimeId == editTimeDto.TimeId).StartTime =
                            new DateTime(2000, 12, 25, Int32.Parse(startTime[0]),
                                Int32.Parse(startTime[1]), 0);
                    }
                    if (!String.IsNullOrEmpty(editTimeDto.EndTime))
                    {
                        string[] endTime = editTimeDto.EndTime.Split("-");
                        editingCourse.Times.FirstOrDefault(t => t.TimeId == editTimeDto.TimeId).EndTime =
                            new DateTime(2000, 12, 25, Int32.Parse(endTime[0]),
                                Int32.Parse(endTime[1]), 0);
                    }

                    if (editTimeDto.WeekDay != null)
                    {
                        editingCourse.Times.FirstOrDefault(t => t.TimeId == editTimeDto.TimeId).WeekDay =
                            editTimeDto.WeekDay;
                    }
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
                        // return error
                    }
                }
            }


            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}