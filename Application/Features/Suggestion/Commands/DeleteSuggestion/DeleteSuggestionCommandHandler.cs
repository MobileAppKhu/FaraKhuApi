using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Suggestion.Commands.RemoveSuggestion
{
    public class RemoveSuggestionCommandHandler : IRequestHandler<RemoveSuggestionCommand>
    {
        private readonly IDatabaseContext _context;

        public RemoveSuggestionCommandHandler(IDatabaseContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(RemoveSuggestionCommand request, CancellationToken cancellationToken)
        {
            var suggestion = await _context.Suggestions.FirstOrDefaultAsync(s =>
                s.SuggestionId == request.SuggestionId, cancellationToken);
            _context.Suggestions.Remove(suggestion);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}