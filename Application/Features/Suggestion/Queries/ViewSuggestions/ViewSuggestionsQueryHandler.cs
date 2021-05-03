using System.Collections.Generic;
using System.Linq;
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

namespace Application.Features.Suggestion.Queries.ViewSuggestions
{
    public class ViewSuggestionsQueryHandler : IRequestHandler<ViewSuggestionsQuery, ViewSuggestionsViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public ViewSuggestionsQueryHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
        }
        public async Task<ViewSuggestionsViewModel> Handle(ViewSuggestionsQuery request, CancellationToken cancellationToken)
        {
            var suggestions = _context.Suggestions.Include(s => s.Sender).ToList();
            return new ViewSuggestionsViewModel
            {
                Suggestions = _mapper.Map<ICollection<SuggestionDto>>(suggestions)
            };
        }
    }
}