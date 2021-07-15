using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Command.UpdateNews
{
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public UpdateNewsCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
        }
        public async Task<Unit> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if(user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
           
            var news = await _context.News.Include(n => n.FileEntity).
                FirstOrDefaultAsync(n => n.NewsId == request.NewsId, cancellationToken);
            if (news == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.NewsNotFound,
                    Message = Localizer["NewsNotFound"]
                });
            }
            /*var oldFile = await _context.Files.FirstOrDefaultAsync(f => f.Id == news.FileId, cancellationToken);

            _context.Files.Remove(oldFile);*/
            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                news.Description = request.Description;
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                news.Title = request.Title;
            }

            if (!string.IsNullOrWhiteSpace(request.FileId))
            {
                var fileEntity = await _context.Files.FirstOrDefaultAsync(f => f.Id == request.FileId,
                    cancellationToken);
                if (fileEntity == null)
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.FileNotFound,
                        Message = Localizer["FileNotFound"]
                    });
                news.FileId = fileEntity.Id;
                news.FileEntity = fileEntity;
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}