using System;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Commands.AddNews;

public class AddNewsCommandHandler : IRequestHandler<AddNewsCommand, AddNewsViewModel>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }
    private IMapper _mapper { get; }

    public AddNewsCommandHandler( IStringLocalizer<SharedResource> localizer,
        IDatabaseContext context, IMapper mapper)
    {
        _context = context;
        Localizer = localizer;
        _mapper = mapper;
    }
    public async Task<AddNewsViewModel> Handle(AddNewsCommand request, CancellationToken cancellationToken)
    {
        var fileEntity = await _context.Files.FirstOrDefaultAsync(f => f.Id == request.FileId,
            cancellationToken);

        if (fileEntity == null)
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.FileNotFound,
                Message = Localizer["FileNotFound"]
            });
            
        Domain.Models.News news = new Domain.Models.News
        {
            Description = request.Description,
            Title = request.Title,
            FileId = request.FileId,
            CreatedDate = DateTime.Now
        };
        await _context.News.AddAsync(news, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AddNewsViewModel
        {
            News = _mapper.Map<NewsDto>(news)
        };
    }
}