using MediatR;

namespace Application.Features.Time.Command.RemoveTime
{
    public class RemoveTimeCommand : IRequest<Unit>
    {
        public string TimeId { get; set; }
    }
}