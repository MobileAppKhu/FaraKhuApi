using MediatR;

namespace Application.Features.User.Queries.SearchProfile
{
    public class SearchProfileQuery : IRequest<SearchProfileViewModel>
    {
        public string UserId { get; set; }
    }
}