using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Poll;
using Application.Features.Notification.SystemCallCommands;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.AddQuestion;

public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, AddQuestionViewModel>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }
    private IMapper _mapper { get; }

    public AddQuestionCommandHandler( IStringLocalizer<SharedResource> localizer, IMapper mapper
        , IDatabaseContext context)
    {
        _context = context;
        Localizer = localizer;
        _mapper = mapper;
    }

    public async Task<AddQuestionViewModel> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        Instructor user = await _context.Instructors.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        var course = await _context.Courses
            .Include(c => c.Instructor)
            .Include(c => c.Polls)
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.CourseId == request.CourseId
                , cancellationToken);

        if (course.Instructor != user)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.Unauthorized,
                Message = Localizer["Unauthorized"]
            });
        }
            
        var poll = new PollQuestion
        {
            QuestionDescription = request.QuestionDescription,
            MultiVote = bool.Parse(request.MultiVote),
            Course = course,
            CourseId = course.CourseId,
            CreatedDate = DateTime.Now,
            Answers = new List<PollAnswer>()
        };

        foreach (var answerDescription in request.Answers)
        {
            poll.Answers.Add(new PollAnswer
            {
                Question = poll,
                QuestionId = poll.QuestionId,
                AnswerDescription = answerDescription
            });
        }
        await _context.PollQuestions.AddAsync(poll, cancellationToken);
            
        foreach (var student in course.Students)
        {
            NotificationAdder.AddNotification(_context,
                Localizer["YouHaveANewCourseEvent"],
                poll.QuestionId, NotificationObjectType.Poll,
                NotificationOperation.NewPoll, student);
        }
            
        await _context.SaveChangesAsync(cancellationToken);

        return new AddQuestionViewModel
        {
            Poll = _mapper.Map<PollQuestionDto>(poll)
        };
    }
}