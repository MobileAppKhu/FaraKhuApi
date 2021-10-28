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

namespace Application.Features.Poll.Commands.RemoveQuestion
{
    public class RemoveQuestionCommandHandler : IRequestHandler<RemoveQuestionCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public RemoveQuestionCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(RemoveQuestionCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Instructor user = await _context.Instructors.FirstOrDefaultAsync(i => i.Id == userId, cancellationToken);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var question = await _context.PollQuestions.Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuestionId == request.QuestionId, cancellationToken);
            _context.PollQuestions.Remove(question);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}