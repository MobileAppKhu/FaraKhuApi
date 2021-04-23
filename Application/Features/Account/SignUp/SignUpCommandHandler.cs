using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.DTOs.Instructor;
using Application.DTOs.Student;
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
                    Message = _localizer.GetString(() => Resources_SharedResource_fa_IR.DuplicateUser),
                    ErrorType = ErrorType.DuplicateUser
                });
            }

            BaseUser user = null;
            switch (request.UserType)
            {
                case UserType.Instructor :
                    user = new Instructor
                    {
                        Email = request.Email.EmailNormalize(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        InstructorId = request.Id
                    };
                    break;
                case UserType.Student :
                    user = new Student
                    {
                        Email = request.Email.EmailNormalize(),
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        StudentId = request.Id
                    };
                    break;
            }

            var result = _userManager.CreateAsync(user);
            if (!result.IsCompletedSuccessfully)
            {
                throw new CustomException(new Error
                {
                    Message = _localizer.GetString(() => Resources_SharedResource_fa_IR.Unexpected),
                    ErrorType = ErrorType.Unexpected
                });
            }

            await _userManager.AddToRoleAsync(user, request.UserType.ToString());
            
            await _signInManager.SignInAsync(user, false);
            if(request.UserType == UserType.Instructor)
                return new SignUpViewModel
                {
                    ProfileDto = _context.Instructors.Where(i => i.InstructorId == request.Id).ProjectTo<InstructorProfileDto>(_mapper.ConfigurationProvider)
                };
            return new SignUpViewModel
            {
                ProfileDto = _context.Students.Where(i => i.StudentId == request.Id).ProjectTo<StudentProfileDto>(_mapper.ConfigurationProvider)
            };
        }
    }
}