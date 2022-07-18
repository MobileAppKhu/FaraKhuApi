using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.DeleteNews;

public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }

    public DeleteNewsCommandHandler(IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
    {
        _context = context;
        Localizer = localizer;
    }

    public async Task<Unit> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
    {
        Domain.Models.News news =
            await _context.News.FirstOrDefaultAsync(n => n.NewsId == request.NewsId, cancellationToken);

        if (news == null)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.NewsNotFound,
                Message = Localizer["NewsNotFound"]
            });
        }

        _context.News.Remove(news);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}