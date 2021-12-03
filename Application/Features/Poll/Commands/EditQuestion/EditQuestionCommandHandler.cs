using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
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

namespace Application.Features.Poll.Commands.EditQuestion
{
    public class EditQuestionCommandHandler : IRequestHandler<EditQuestionCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper Mapper { get; }

        public EditQuestionCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            Mapper = mapper;
        }
        public async Task<Unit> Handle(EditQuestionCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Instructor user = await _context.Instructors.
                FirstOrDefaultAsync(i => i.Id == userId, cancellationToken);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var questionObj =
                await _context.PollQuestions.Include(question => question.Course)
                    .ThenInclude(course => course.Instructor)
                    .Include(question => question.Answers)
                    .FirstOrDefaultAsync(pollQuestion => pollQuestion.QuestionId == request.QuestionId, cancellationToken);
            if (questionObj == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.QuestionNotFound,
                    Message = Localizer["QuestionNotFound"]
                });
            }

            if (questionObj.Course.Instructor != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            if (request.DeleteAnswers.Count != 0)
            {
                var deleteAnswers = await _context.PollAnswers.Where(answer =>
                        answer.QuestionId == questionObj.QuestionId && request.DeleteAnswers.Contains(answer.AnswerId))
                    .ToListAsync(cancellationToken);
                if (deleteAnswers.Count != request.DeleteAnswers.Count)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.AnswerNotFound,
                        Message = Localizer["AnswerNotFound"]
                    });
                }
                foreach (var answer in deleteAnswers)
                {
                    questionObj.Answers.Remove(answer);
                }
            }

            if (request.AddAnswers.Count != 0)
            {
                foreach (var answerDescription in request.AddAnswers)
                {
                    questionObj.Answers.Add(new PollAnswer
                    {
                        Question = questionObj,
                        QuestionId = questionObj.QuestionId,
                        AnswerDescription = answerDescription
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(request.QuestionDescription))
            {
                questionObj.QuestionDescription = request.QuestionDescription;
            }

            if (!string.IsNullOrWhiteSpace(request.IsOpen))
            {
                questionObj.IsOpen = request.IsOpen.Equals("t");
            }

            if (!string.IsNullOrWhiteSpace(request.MultiVote))
            {
                questionObj.MultiVote = request.MultiVote.Equals("t");
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}