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

namespace Application.Features.Poll.Commands.RetractVote
{
    public class RetractVoteCommandHandler : IRequestHandler<RetractVoteCommand, RetractVoteViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public RetractVoteCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }

        public async Task<RetractVoteViewModel> Handle(RetractVoteCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Students.Include(s => s.PollAnswers).
                FirstOrDefaultAsync(s => s.Id == request.UserId, cancellationToken);
            
            var answer = await _context.PollAnswers.Include(a => a.Voters)
                .Include(a => a.Question).FirstOrDefaultAsync(a => a.AnswerId == request.AnswerId, cancellationToken);
            if (!answer.Question.IsOpen)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.PollIsNotOpen,
                    Message = Localizer["PollIsNotOpen"]
                });
            user.PollAnswers.Remove(answer);
            await _context.SaveChangesAsync(cancellationToken);

            return new RetractVoteViewModel();
        }
    }
}