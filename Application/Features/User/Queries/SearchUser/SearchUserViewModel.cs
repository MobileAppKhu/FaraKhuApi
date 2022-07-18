using System.Collections.Generic;
using Application.DTOs.User;

namespace Application.Features.User.Queries.SearchUser;

public class SearchUserViewModel
{
    public List<ProfileDto> Users { get; set; }
    public int SearchLength { get; set; }
}