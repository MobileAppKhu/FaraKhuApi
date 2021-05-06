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

namespace Application.Features.Suggestion.Command.CreateSuggestion
{
    public class CreateSuggestionCommandHandler : IRequestHandler<CreateSuggestionCommand, CreateSuggestionViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public CreateSuggestionCommandHandler(IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IDatabaseContext context)
        {
            _context = context;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<CreateSuggestionViewModel> Handle(CreateSuggestionCommand request, CancellationToken cancellationToken)
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
            return new CreateSuggestionViewModel
            {
                Suggestion = _mapper.Map<SuggestionDto>(suggestion)
            };
        }
    }
}