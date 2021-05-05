
using System.Linq;
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

        private readonly IDatabaseContext _context;

        public SignUpCommandHandler(UserManager<BaseUser> userManager, IStringLocalizer<SharedResource> localizer,
            SignInManager<BaseUser> signInManager, IMapper mapper, IDatabaseContext context)
        {
            _userManager = userManager;
            _localizer = localizer;
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
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

            await _userManager.AddToRoleAsync(user, request.UserType.ToString().Normalize());

            //await _signInManager.SignInAsync(user, false);

            
           
            return new SignUpViewModel
            {
                ProfileDto = _mapper.Map<ProfileDto>(user)
            };
            
        }
    }
}