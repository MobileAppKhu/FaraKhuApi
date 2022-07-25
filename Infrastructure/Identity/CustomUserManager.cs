using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.BaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Identity;

public class CustomUserManager : UserManager<BaseUser>
{
    public CustomUserManager(IUserStore<BaseUser> store, IOptions<IdentityOptions> optionsAccessor, 
        IPasswordHasher<BaseUser> passwordHasher, IEnumerable<IUserValidator<BaseUser>> userValidators, 
        IEnumerable<IPasswordValidator<BaseUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<BaseUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
            
        UserValidators.Clear();
        UserValidators.Add(new CustomUserValidator());
    }
        

    internal class CustomUserValidator : UserValidator<BaseUser>
    {
        public CustomUserValidator(IdentityErrorDescriber errors = null) =>
            Describer = errors ?? new IdentityErrorDescriber();

        private new IdentityErrorDescriber Describer { get; }

        public override async Task<IdentityResult> ValidateAsync(
            UserManager<BaseUser> manager,
            BaseUser user)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var errors = new List<IdentityError>();
            if (manager.Options.User.RequireUniqueEmail)
                await ValidateEmail(manager, user, errors);
            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }

        private async Task ValidateEmail(
            UserManager<BaseUser> manager,
            BaseUser user,
            List<IdentityError> errors)
        {
            var email = await manager.GetEmailAsync(user);
            if (string.IsNullOrWhiteSpace(email))
                errors.Add(Describer.InvalidEmail(email));
            else if (!new EmailAddressAttribute().IsValid(email))
            {
                errors.Add(Describer.InvalidEmail(email));
            }
            else
            {
                var byEmailAsync = await manager.FindByEmailAsync(email);
                var flag = byEmailAsync != null;
                if (flag)
                {
                    var a = await manager.GetUserIdAsync(byEmailAsync);
                    flag = !string.Equals(a, await manager.GetUserIdAsync(user));
                }

                if (!flag)
                    return;
                errors.Add(Describer.DuplicateEmail(email));
            }
        }
    }
        
}