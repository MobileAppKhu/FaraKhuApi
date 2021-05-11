using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Application.Features.File.Commands.Upload
{
    public class UploadCommand: IRequest<UploadViewModel>
    {
        public IFormFile File { get; set; }
        public FileType? FileType { get; set; }

    }
}