using System.Threading;
using System.Threading.Tasks;
using Domain.BaseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Account.SignOut
{
    public class SignOutCommandHandler : IRequest<SignOutCommand>
    {
        private SignInManager<BaseUser> _signInManager { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public SignOutCommandHandler(IHttpContextAccessor httpContextAccessor, SignInManager<BaseUser> signInManager)
        {
            _signInManager = signInManager;
            HttpContextAccessor = httpContextAccessor;
        }


        public async Task<Unit> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            HttpContextAccessor.HttpContext?.Response.Headers.Remove("Roles");
            return Unit.Value;
        }
    }
}