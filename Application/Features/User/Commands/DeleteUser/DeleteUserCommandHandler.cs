using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> _localizer { get; }

        public DeleteUserCommandHandler(IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
        {
            _context = context;
            _localizer = localizer;
        }
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == request.UserId,
                                                                    cancellationToken);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.UserNotFound,
                    Message = _localizer["UserNotFound"]
                }); 
            }

            switch (user.UserType)
            {
                case UserType.Student:
                    var std = await _context.Students
                        .Include(student => student.Courses)
                        .Include(student => student.PollAnswers)
                        .Include(student => student.Favourites)
                        .Include(student => student.Notifications)
                        .Include(student => student.Offers)
                        .Include(student => student.Events)
                        .FirstOrDefaultAsync(student => student.Id == request.UserId, cancellationToken);
                    _context.Favourites.RemoveRange(std.Favourites);
                    _context.Notifications.RemoveRange(std.Notifications);
                    _context.Offers.RemoveRange(std.Offers);
                    _context.Events.RemoveRange(std.Events);
                    _context.Students.Remove(std);
                    break;
                case UserType.Instructor:
                    var ins = await _context.Instructors
                        .Include(instructor => instructor.Courses)
                        .ThenInclude(course => course.Polls)
                        .Include(instructor => instructor.Favourites)
                        .Include(instructor => instructor.Notifications)
                        .Include(instructor => instructor.Offers)
                        .Include(instructor => instructor.Events)
                        .FirstOrDefaultAsync(instructor => instructor.Id == request.UserId, cancellationToken);
                    _context.Favourites.RemoveRange(ins.Favourites);
                    _context.Notifications.RemoveRange(ins.Notifications);
                    _context.Offers.RemoveRange(ins.Offers);
                    _context.Events.RemoveRange(ins.Events);
                    _context.Instructors.Remove(ins);
                    break;
                default:
                    _context.BaseUsers.Remove(user);
                    break;
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}