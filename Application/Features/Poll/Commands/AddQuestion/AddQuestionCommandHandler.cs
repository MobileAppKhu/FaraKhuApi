﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Offer;
using Application.DTOs.Poll;
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

namespace Application.Features.Poll.Commands.AddQuestion
{
    public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, AddQuestionViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public AddQuestionCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<AddQuestionViewModel> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Instructor user = await _context.Instructors.FirstOrDefaultAsync(i => i.Id == userId, cancellationToken);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });

            var course = await _context.Courses.Include(c => c.Instructor)
                .Include(c => c.Polls).FirstOrDefaultAsync(c => c.CourseId == request.CourseId
                , cancellationToken);
            var poll = new PollQuestion
            {
                QuestionDescription = request.QuestionDescription,
                MultiVote = bool.Parse(request.MultiVote),
                Course = course,
                CourseId = course.CourseId
            };
            await _context.PollQuestions.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AddQuestionViewModel
            {
                Poll = _mapper.Map<PollQuestionDto>(poll)
            };
        }
    }
}