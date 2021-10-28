using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Poll;
using Application.Resources;
using AutoMapper;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Queries.SearchAvailablePolls
{
    public class SearchPollsQueryHandler : IRequestHandler<SearchPollsQuery, SearchPollsViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IMapper _mapper { get; }

        public SearchPollsQueryHandler( IStringLocalizer<SharedResource> localizer,IMapper mapper
                                    , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            _mapper = mapper;
        }
        public async Task<SearchPollsViewModel> Handle(SearchPollsQuery request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.Include(c => c.Polls)
                .FirstOrDefaultAsync(c => c.CourseId == request.CourseId, cancellationToken);
            int searchLength = course.Polls.Count;
            course.Polls = course.Polls.Skip(request.Start).Take(request.Step).ToList();
            return new SearchPollsViewModel
            {
                Polls = _mapper.Map<ICollection<PollQuestionShortDto>>(course.Polls),
                SearchLenght = searchLength
            };
        }
    }
}