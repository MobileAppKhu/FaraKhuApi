using MediatR;

namespace Application.Features.Suggestion.Command.RemoveSuggestion
{
    public class RemoveSuggestionCommand : IRequest<Unit>
    {
        public string SuggestionId { get; set; }
    }
}