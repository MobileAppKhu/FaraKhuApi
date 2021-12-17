using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Application.Features.Account.SignUp;
using Application.Resources;
using Application.Utilities;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, AddUserViewModel>
    {
        private readonly IDatabaseContext _context;
        
        private IStringLocalizer<SharedResource> _localizer { get; }
        
        private IHttpContextAccessor HttpContextAccessor { get; }
        
        private UserManager<BaseUser> _userManager { get; }
        private IMapper _mapper { get; }

        public AddUserCommandHandler( IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, UserManager<BaseUser> userManager, IMapper mapper
            , IDatabaseContext context)
        {
            _context = context;
            _localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<AddUserViewModel> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var duplicateUser = await _userManager.FindByEmailAsync(request.Email.EmailNormalize());
            if (duplicateUser != null)
            {
                throw new CustomException(new Error
                {
                    Message = _localizer["DuplicateUser"],
                    ErrorType = ErrorType.DuplicateUser
                });
            }
            //Should be Duplicate StudentId not Duplicate Email
            switch (request.UserType)
            {
                case UserType.Instructor:
                    var duplicateInstructor = _context.Instructors.
                        FirstOrDefault(i => i.InstructorId == request.Id);
                    if(duplicateInstructor != null)
                        throw new CustomException(new Error
                        {
                            Message = _localizer["DuplicateUser"],
                            ErrorType = ErrorType.DuplicateUser
                        });
                    break;
                case UserType.Student:
                    var duplicateStudent = _context.Students.
                        FirstOrDefault(s => s.StudentId == request.Id);
                    if(duplicateStudent != null)
                        throw new CustomException(new Error
                        {
                            Message = _localizer["DuplicateUser"],
                            ErrorType = ErrorType.DuplicateUser
                        });
                    break;
            }
            
            // get smiley.png(default avatar picture)
            var smiley =
                await _context.Files.FirstOrDefaultAsync(entity => entity.Id == "smiley.png", cancellationToken);

            BaseUser user = null;
            switch (request.UserType)
            {
                case UserType.Instructor:
                    user = new Instructor
                    {
                        Email = request.Email.EmailNormalize(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        InstructorId = request.Id,
                        AvatarId = smiley.Id
                    };
                    break;
                case UserType.Student:
                    user = new Student
                    {
                        Email = request.Email.EmailNormalize(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        StudentId = request.Id,
                        AvatarId = smiley.Id
                    };
                    break;
                case UserType.PROfficer:
                    user = new BaseUser
                    {
                        Email = request.Email.EmailNormalize(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        AvatarId = smiley.Id
                    };
                    break;
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                throw new CustomException(new Error
                {
                    Message = _localizer["Unexpected"],
                    ErrorType = ErrorType.Unexpected
                });
            }

            await _userManager.AddToRoleAsync(user, request.UserType.ToString().Normalize());
            
            return new AddUserViewModel
            {
                ProfileDto = _mapper.Map<ProfileDto>(user)
            };
        }
    }
}