using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Offer;
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

namespace Application.Features.Poll.Commands.Vote
{
    public class VoteCommandHandler : IRequestHandler<VoteCommand, VoteViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public VoteCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<VoteViewModel> Handle(VoteCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Student user = await _context.Students.Include(s => s.PollAnswers).
                FirstOrDefaultAsync(s => s.Id == userId, cancellationToken);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            
            var answer = await _context.PollAnswers
                .Include(a => a.Voters)
                .Include(a => a.Question)
                .ThenInclude(a => a.Course)
                .ThenInclude(course => course.Students)
                .FirstOrDefaultAsync(a => a.AnswerId == request.AnswerId, cancellationToken);

            if (answer == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.AnswerNotFound,
                    Message = Localizer["AnswerNotFound"]
                });
            }
            
            if (!answer.Question.Course.Students.Contains(user))
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }
            if (!answer.Question.IsOpen)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.PollIsNotOpen,
                    Message = Localizer["PollIsNotOpen"]
                });
            if (!answer.Question.MultiVote)
                foreach (var temp in answer.Question.Answers)
                    if (temp.Voters.Contains(user))
                    {
                        temp.Voters.Remove(user);
                        break;
                    }
            
            user.PollAnswers.Add(answer);
            await _context.SaveChangesAsync(cancellationToken);

            return new VoteViewModel();
        }
    }
}