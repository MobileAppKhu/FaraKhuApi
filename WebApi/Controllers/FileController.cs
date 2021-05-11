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
            var viewModel = await _mediator.Send(request);
            var stream = System.IO.File.OpenRead(viewModel.Path);
            return File(stream, viewModel.ContentType ?? "application/octet-stream", viewModel.Name);
        }
    }
}