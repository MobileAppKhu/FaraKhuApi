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

namespace Application.Features.Poll.Commands.CreateAnswer
{
    public class CreateAnswerCommandHandler : IRequestHandler<CreateAnswerCommand, CreateAnswerViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public CreateAnswerCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<CreateAnswerViewModel> Handle(CreateAnswerCommand request, CancellationToken cancellationToken)
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
            var question = await _context.PollQuestions.Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuestionId == request.QuestionId, cancellationToken);
            var answer = new PollAnswer
            {
                AnswerDescription = request.AnswerDescription,
                Question = question,
                QuestionId = question.QuestionId
            };
            await _context.PollAnswers.AddAsync(answer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateAnswerViewModel
            {
                Answer = _mapper.Map<PollAnswerDto>(answer)
            };
        }
    }
}