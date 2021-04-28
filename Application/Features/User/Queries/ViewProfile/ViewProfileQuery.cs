using MediatR;

namespace Application.Features.User.Queries.ViewProfile
{
    public class ViewProfileQuery : IRequest<ViewProfileViewModel>
    {
        public string UserId { get; set; }
    }
}