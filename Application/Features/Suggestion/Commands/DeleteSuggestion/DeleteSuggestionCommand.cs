using MediatR;

namespace Application.Features.Suggestion.Commands.RemoveSuggestion
{
    public class RemoveSuggestionCommand : IRequest<Unit>
    {
        public string SuggestionId { get; set; }
    }
}