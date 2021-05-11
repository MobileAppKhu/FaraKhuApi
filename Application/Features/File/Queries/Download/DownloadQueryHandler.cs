using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.FileEntity;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Application.Features.File.Queries.Download
{
    public class DownloadQueryHandler : IRequestHandler<DownloadQuery, DownloadViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }
        public IConfiguration _config { get; set; }

        public DownloadQueryHandler( IStringLocalizer<SharedResource> localizer,
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

        public async Task<DownloadViewModel> Handle(DownloadQuery request, CancellationToken cancellationToken)
        {
            var id = request.FileId;
            if (string.IsNullOrWhiteSpace(id))
                throw new CustomException(new Error
                    {ErrorType = ErrorType.InvalidInput, Message = Localizer["InvalidFileID"]});

            var file = _context.Files.Find(id);
            if (file == null)
                throw new CustomException(new Error
                    {ErrorType = ErrorType.FileNotFound, Message = Localizer["FileNotFound"]});

            var path = _config["StorePath"] + id;
            
            return new DownloadViewModel
            {
                DownloadDto = new DownloadDto
                {
                    Name = file.Name, Path = path, ContentType = file.ContentType
                }
            };
            
        }
    }
}