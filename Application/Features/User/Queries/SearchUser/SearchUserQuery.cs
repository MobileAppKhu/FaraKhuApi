using Domain.Enum;
using MediatR;

namespace Application.Features.User.Queries.SearchUser;

public class SearchUserQuery : IRequest<SearchUserViewModel>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string LinkedIn { get; set; }
    public string GoogleScholar { get; set; }
    public int Start { get; set; }
    public int Step { get; set; }
    public UserColumn UserColumn { get; set; }
    public bool OrderDirection { get; set; }
}