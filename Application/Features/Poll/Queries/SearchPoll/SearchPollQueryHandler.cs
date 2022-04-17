using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Poll;
using Application.Resources;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Queries.SearchPoll
{
    public class SearchPollQueryHandler : IRequestHandler<SearchPollQuery, SearchPollViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper _mapper { get; }

        public SearchPollQueryHandler(IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SearchPollViewModel> Handle(SearchPollQuery request, CancellationToken cancellationToken)
        {
            var questionPoll = await _context.PollQuestions.Include(q => q.Answers)
                .ThenInclude(a => a.Voters)
                .FirstOrDefaultAsync(q => q.QuestionId == request.QuestionId, cancellationToken);

            return new SearchPollViewModel
            {
                Question = _mapper.Map<PollQuestionDto>(questionPoll)
            };
        }
    }
}