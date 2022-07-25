using MediatR;

namespace Application.Features.News.Commands.EditNews;

public class EditNewsCommand : IRequest<Unit>
{
    public string NewsId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FileId { get; set; }
}