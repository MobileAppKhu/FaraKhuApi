using System.IO;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Application.Features.File.Commands.Upload
{
    public class UploadCommandHandler : IRequestHandler<UploadCommand, UploadViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public IConfiguration _config { get; set; }

        public UploadCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
            _config = configuration;
        }

        public async Task<UploadViewModel> Handle(UploadCommand request, CancellationToken cancellationToken)
        {
            var file = new Domain.BaseModels.FileEntity
            {
                Name = request.File.FileName,
                Size = request.File.Length,
                ContentType = request.File.ContentType,
                Type = FileType.Image
            };

            await _context.Files.AddAsync(file, cancellationToken);
            
            var stream = new MemoryStream();
            await request.File.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;
            return new UploadViewModel();
            /*var file = request.Form.Files.FirstOrDefault();
            if (file == null)
                throw new CustomException(new Error
                    {ErrorType = ErrorType.InvalidInput, Message = Localizer["InvalidFile"]});

            var fileEntity = new FileEntity
            {
                Name = file.FileName,
                Size = file.Length,
                ContentType = file.ContentType,
            };

            await _context.Files.AddAsync(fileEntity,cancellationToken);

            var fileId = fileEntity.Id;
            var path = _config["StorePath"] + fileId;

            await using var stream = System.IO.File.Create(path);
            await file.CopyToAsync(stream,cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            return new UploadViewModel
            {
                FileId = fileId
            };*/
        }
    }
}