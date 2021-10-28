using System.Collections.Generic;
using Application.DTOs.Suggestion;

namespace Application.Features.Suggestion.Queries.SearchSuggestions
{
    public class SearchSuggestionsViewModel
    {
        public ICollection<SuggestionDto> Suggestions { get; set; }
    }
}