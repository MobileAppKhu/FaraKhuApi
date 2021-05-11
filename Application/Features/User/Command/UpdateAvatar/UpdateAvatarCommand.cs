using MediatR;

namespace Application.Features.User.Command.UpdateAvatar
{
    public class UpdateAvatarCommand: IRequest<Unit>
    {
        public string FileId { get; set; }
        public bool DeleteAvatar { get; set; }
    }
}