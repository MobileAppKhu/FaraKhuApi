using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.File.Commands.Upload;
using Application.Features.File.Queries.Download;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FileController : ControllerBase
    {
        private IMediator _mediator;
        public FileController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UploadViewModel),200)]
        public async Task<IActionResult> Upload(IFormCollection collection)
        {
            return Ok(await _mediator.Send(new UploadCommand
            {
                File = collection?.Files.FirstOrDefault(),
                FileType = Enum.TryParse<FileType>(collection["type"], out var fileType) ? fileType : null
            }));
        }

        [HttpPost]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        public async Task<IActionResult> Download(DownloadQuery request)
        {
            var downloadFile = (await _mediator.Send(request)).DownloadDto;

            if (downloadFile.ContentType?.StartsWith("image") ?? false)
                HttpContext.Response.Headers.Append("Cache-Control",
                    "max-age=" + new TimeSpan(30, 0, 0, 0)
                        .TotalSeconds.ToString("0"));

            var stream = System.IO.File.OpenRead(downloadFile.Path);
            return File(stream, downloadFile.ContentType ?? "application/octet-stream", downloadFile.Name);
        }
    }
}