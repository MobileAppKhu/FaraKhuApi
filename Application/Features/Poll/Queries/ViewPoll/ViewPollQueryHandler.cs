using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Poll;
using Application.Resources;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Queries.ViewPoll
{
    public class ViewPollQueryHandler : IRequestHandler<ViewPollQuery, ViewPollViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IMapper _mapper { get; }

        public ViewPollQueryHandler( IStringLocalizer<SharedResource> localizer,IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            _mapper = mapper;
        }
        public async Task<ViewPollViewModel> Handle(ViewPollQuery request, CancellationToken cancellationToken)
        {
            var questionPoll = await _context.PollQuestions.Include(q => q.Answers)
                .ThenInclude(a => a.Voters)
                .FirstOrDefaultAsync(q => q.QuestionId == request.QuestionId, cancellationToken);

            return new ViewPollViewModel
            {
                Question = _mapper.Map<PollQuestionDto>(questionPoll)
            };
        }
    }
}