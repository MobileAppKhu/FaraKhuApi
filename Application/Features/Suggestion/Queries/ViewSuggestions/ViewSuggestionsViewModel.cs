using System.Collections.Generic;
using Application.DTOs.Suggestion;

namespace Application.Features.Suggestion.Queries.ViewSuggestions
{
    public class ViewSuggestionsViewModel
    {
        public ICollection<SuggestionDto> Suggestions { get; set; }
    }
}