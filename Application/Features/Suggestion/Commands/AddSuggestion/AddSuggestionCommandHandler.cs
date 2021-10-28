using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Suggestion;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Suggestion.Commands.AddSuggestion
{
    public class AddSuggestionCommandHandler : IRequestHandler<AddSuggestionCommand, AddSuggestionViewModel>
    {
        private readonly IDatabaseContext _context;
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }

        public AddSuggestionCommandHandler(IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<AddSuggestionViewModel> Handle(AddSuggestionCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            var suggestion = new Domain.Models.Suggestion
            {
                Detail = request.Detail,
                Sender = user,
                SenderId = userId
            };
            await _context.Suggestions.AddAsync(suggestion, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new AddSuggestionViewModel
            {
                Suggestion = _mapper.Map<SuggestionDto>(suggestion)
            };
        }
    }
}