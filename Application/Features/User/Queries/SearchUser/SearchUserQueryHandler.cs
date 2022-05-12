using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.User;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.User.Queries.SearchUser
{
    public class SearchUserQueryHandler : IRequestHandler<SearchUserQuery, SearchUserViewModel>
    {
        
        private readonly IMapper _mapper;
        private readonly IDatabaseContext _context;


        public SearchUserQueryHandler(IMapper mapper, IDatabaseContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<SearchUserViewModel> Handle(SearchUserQuery request, CancellationToken cancellationToken)
        {
            IQueryable<BaseUser> usersQueryable = _context.BaseUsers.Include(baseUser => baseUser.Favourites);

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                usersQueryable = usersQueryable.Where(bu => bu.FirstName.Contains(request.FirstName));
            }
            
            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                usersQueryable = usersQueryable.Where(bu => bu.LastName.Contains(request.LastName));
            }
            
            if (!string.IsNullOrWhiteSpace(request.GoogleScholar))
            {
                usersQueryable = usersQueryable.Where(bu => bu.GoogleScholar.Contains(request.GoogleScholar));
            }
            
            if (!string.IsNullOrWhiteSpace(request.LinkedIn))
            {
                usersQueryable = usersQueryable.Where(bu => bu.LinkedIn.Contains(request.LinkedIn));
            }

            switch (request.UserColumn)
            {
                case UserColumn.Id:
                    usersQueryable = request.OrderDirection
                        ? usersQueryable.OrderBy(bu => bu.Id)
                        : usersQueryable.OrderByDescending(bu => bu.Id);
                    break;
                case UserColumn.Firstname:
                    usersQueryable = request.OrderDirection
                        ? usersQueryable.OrderBy(bu => bu.FirstName)
                            .ThenBy(bu => bu.Id)
                        : usersQueryable.OrderByDescending(bu => bu.FirstName)
                            .ThenByDescending(bu => bu.Id);
                    break;
                case UserColumn.Lastname:
                    usersQueryable = request.OrderDirection
                        ? usersQueryable.OrderBy(bu => bu.LastName)
                            .ThenBy(bu => bu.Id)
                        : usersQueryable.OrderByDescending(bu => bu.LastName)
                            .ThenByDescending(bu => bu.Id);
                    break;
                case UserColumn.GoogleScholar:
                    usersQueryable = request.OrderDirection
                        ? usersQueryable.OrderBy(bu => bu.GoogleScholar)
                            .ThenBy(bu => bu.Id)
                        : usersQueryable.OrderByDescending(bu => bu.GoogleScholar)
                            .ThenByDescending(bu => bu.Id);
                    break;
                case UserColumn.LinkedIn:
                    usersQueryable = request.OrderDirection
                        ? usersQueryable.OrderBy(bu => bu.LinkedIn)
                            .ThenBy(bu => bu.Id)
                        : usersQueryable.OrderByDescending(bu => bu.LinkedIn)
                            .ThenByDescending(bu => bu.Id);
                    break;
            }

            int searchLength = await usersQueryable.CountAsync(cancellationToken);

            List<BaseUser> baseUsers = await usersQueryable
                .Skip(request.Start)
                .Take(request.Step)
                .ToListAsync(cancellationToken);

            return new SearchUserViewModel()
            {
                Users = _mapper.Map<List<ProfileDto>>(baseUsers),
                SearchLength = searchLength
            };
        }
    }
}