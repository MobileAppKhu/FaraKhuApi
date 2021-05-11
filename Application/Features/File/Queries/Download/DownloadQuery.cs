using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.File.Queries.Download
{
    public class DownloadQuery: IRequest<DownloadViewModel>
    {
        public string Id { get; set; }
    }
}