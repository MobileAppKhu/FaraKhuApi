using MediatR;

namespace Application.Features.User.Commands.UpdateAvatar
{
    public class UpdateAvatarCommand: IRequest<Unit>
    {
        public string FileId { get; set; }
        public bool DeleteAvatar { get; set; }
    }
}