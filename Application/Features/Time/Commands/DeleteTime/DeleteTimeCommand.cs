using MediatR;

namespace Application.Features.Time.Commands.DeleteTime
{
    public class DeleteTimeCommand : IRequest<Unit>
    {
        public string TimeId { get; set; }
    }
}