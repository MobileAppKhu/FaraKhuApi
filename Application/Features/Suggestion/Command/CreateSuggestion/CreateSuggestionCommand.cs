using MediatR;

namespace Application.Features.Suggestion.Command.CreateSuggestion
{
    public class CreateSuggestionCommand : CreateSuggestionViewModel, IRequest<CreateSuggestionViewModel>
    {
        public string Detail { get; set; }
    }
}