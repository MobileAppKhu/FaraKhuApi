using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.News;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Queries.ViewIndividualNews
{
    public class ViewIndividualNewsQueryHandler : IRequestHandler<ViewIndividualNewsQuery, ViewIndividualNewsViewModel>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }
        private UserManager<BaseUser> UserManager { get; }
        private IMapper _mapper { get; }

        public ViewIndividualNewsQueryHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            UserManager = userManager;
            _mapper = mapper;
        }
        public async Task<ViewIndividualNewsViewModel> Handle(ViewIndividualNewsQuery request, CancellationToken cancellationToken)
        {
            var news = await _context.News.FirstOrDefaultAsync(n => n.NewsId == request.NewsId, cancellationToken);
            if(news == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.NewsNotFound,
                    Message = Localizer["NewsNotFound"]
                });
            return new ViewIndividualNewsViewModel
            {
                News = _mapper.Map<NewsDto>(news)
            };
        }
    }
}