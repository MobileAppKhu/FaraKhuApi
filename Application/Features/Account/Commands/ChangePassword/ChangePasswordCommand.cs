using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Account.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<Unit>
{
    [JsonIgnore]
    public string UserId { get; set; }
    public string OldPassword { get; set; }
        
    public string NewPassword { get; set; }
}