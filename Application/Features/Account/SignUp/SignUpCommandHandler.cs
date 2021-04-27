
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.User;
using Application.Resources;
using Application.Utilities;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;


namespace Application.Features.Account.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand,SignUpViewModel>
    {
        private UserManager<BaseUser> _userManager { get; }
        
        private IStringLocalizer<SharedResource> _localizer { get; }
        
        private SignInManager<BaseUser> _signInManager { get; }
        
        private IMapper _mapper { get; }

        public SignUpCommandHandler(UserManager<BaseUser> userManager, IStringLocalizer<SharedResource> localizer,
            SignInManager<BaseUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _localizer = localizer;
            _signInManager = signInManager;
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
                case UserType.Instructor:
                    user = new Instructor
                    {
                        Email = request.Email.EmailNormalize(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        InstructorId = request.Id
                    };
                    break;
                case UserType.Student:
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