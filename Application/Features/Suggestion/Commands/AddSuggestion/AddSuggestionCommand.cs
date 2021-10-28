using MediatR;

namespace Application.Features.Suggestion.Commands.AddSuggestion
{
    public class AddSuggestionCommand : IRequest<AddSuggestionViewModel>
    {
        public string Detail { get; set; }
    }
}