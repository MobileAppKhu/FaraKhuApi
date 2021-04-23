using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.DTOs.Instructor;
using Application.DTOs.Student;
using Application.DTOs.User;
using Application.Resources;
using Application.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.Features.Account.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand,SignUpViewModel>
    {
        private DatabaseContext _context { get; }
        private UserManager<BaseUser> _userManager { get; }
        
        private IStringLocalizer<SharedResource> _localizer { get; }
        
        private SignInManager<BaseUser> _signInManager { get; }
        
        private IMapper _mapper { get; }

        public SignUpCommandHandler(UserManager<BaseUser> userManager, IStringLocalizer<SharedResource> localizer,
            SignInManager<BaseUser> signInManager, DatabaseContext context, IMapper mapper)
        {
            _userManager = userManager;
            _localizer = localizer;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
        }
        public async Task<SignUpViewModel> Handle(SignUpCommand request, CancellationToken cancellationToken)
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

            BaseUser user = null;
            switch (request.UserType)
            {
                case UserType.INSTRUCTOR:
                    user = new Instructor
                    {
                        Email = request.Email.EmailNormalize(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        InstructorId = request.Id
                    };
                    break;
                case UserType.STUDENT:
                    user = new Student
                    {
                        Email = request.Email.EmailNormalize(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        StudentId = request.Id
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

            await _userManager.AddToRoleAsync(user, request.UserType.ToString());

            await _signInManager.SignInAsync(user, false);

            
           
            return new SignUpViewModel
            {
                ProfileDto = _mapper.Map<ProfileDto>(user)
            };
            
        }
    }
}