using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.File.Commands.Upload;

public class UploadCommand: IRequest<UploadViewModel>
{
    public IFormFile File { get; set; }
    public FileType? FileType { get; set; }

}