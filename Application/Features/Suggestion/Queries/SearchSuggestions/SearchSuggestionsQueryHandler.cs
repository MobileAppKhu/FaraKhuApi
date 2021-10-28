using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Suggestion;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Suggestion.Queries.SearchSuggestions
{
    public class SearchSuggestionsQueryHandler : IRequestHandler<SearchSuggestionsQuery, SearchSuggestionsViewModel>
    {
        private readonly IDatabaseContext _context;
        private IMapper _mapper { get; }

        public SearchSuggestionsQueryHandler(IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<SearchSuggestionsViewModel> Handle(SearchSuggestionsQuery request, CancellationToken cancellationToken)
        {
            var suggestions = await _context.Suggestions.Include(s => s.Sender).
                ToListAsync(cancellationToken);
            return new SearchSuggestionsViewModel
            {
                Suggestions = _mapper.Map<ICollection<SuggestionDto>>(suggestions)
            };
        }
    }
}