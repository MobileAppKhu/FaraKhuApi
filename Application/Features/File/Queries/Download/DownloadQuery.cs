using MediatR;

namespace Application.Features.File.Queries.Download;

public class DownloadQuery: IRequest<DownloadViewModel>
{
    public string FileId { get; set; }
}