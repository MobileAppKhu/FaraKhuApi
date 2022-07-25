using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Poll;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Poll.Queries.SearchAvailablePolls;

public class SearchPollsQueryHandler : IRequestHandler<SearchPollsQuery, SearchPollsViewModel>
{
    private readonly IDatabaseContext _context;
    private IMapper _mapper { get; }

    public SearchPollsQueryHandler(IMapper mapper
        , IDatabaseContext context)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<SearchPollsViewModel> Handle(SearchPollsQuery request, CancellationToken cancellationToken)
    {
        List<PollQuestion> polls = await _context.PollQuestions.Include(question => question.Answers)
            .Where(question => question.CourseId == request.CourseId)
            .OrderByDescending(question => question.CreatedDate)
            .Take(request.Step)
            .Skip(request.Start).ToListAsync(cancellationToken);
        int searchLength = await _context.PollQuestions.Where(question => question.CourseId == request.CourseId)
            .CountAsync(question => question.CourseId == request.CourseId, cancellationToken);
        return new SearchPollsViewModel
        {
            Polls = _mapper.Map<List<PollQuestionShortDto>>(polls),
            SearchLength = searchLength
        };
    }
}