using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.DeleteQuestion
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public DeleteQuestionCommandHandler(IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }
        public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            Instructor user = await _context.Instructors.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }
            var question = await _context.PollQuestions
                .Include(q => q.Answers)
                .Include(q => q.Course)
                .ThenInclude(q => q.Instructor)
                .FirstOrDefaultAsync(q => q.QuestionId == request.QuestionId, cancellationToken);
            if (question == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.QuestionNotFound,
                    Message = Localizer["QuestionNotFound"]
                });
            }
            if (question.Course.Instructor != user && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            _context.PollAnswers.RemoveRange(question.Answers);
            _context.PollQuestions.Remove(question);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}