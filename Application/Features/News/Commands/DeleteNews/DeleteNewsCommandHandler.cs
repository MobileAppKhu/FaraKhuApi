using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Resources;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.DeleteNews
{
    public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        public DeleteNewsCommandHandler( IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }
        public async Task<Unit> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            Domain.Models.News news = await _context.News.
                FirstOrDefaultAsync(n => n.NewsId == request.NewsId, cancellationToken);
            if (news != null)
            {
                _context.News.Remove(news);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return Unit.Value;
        }
    }
}