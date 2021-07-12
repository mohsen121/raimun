using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Raimun.Core.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Raimun.Core.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private IAppDb _appDb;

        public CreateUserCommandValidator(IAppDb appDb)
        {
            _appDb = appDb;

            RuleFor(x => x.FirstName)
                .NotEmpty().NotNull().MinimumLength(3);
            RuleFor(x => x.LastName)
                .NotEmpty().NotNull().MinimumLength(3);
            RuleFor(x => x.UserName)
                .NotEmpty()
                .NotNull()
                .Custom((prop, context) =>
                {
                    if (!Regex.IsMatch(prop, "^[a-zA-Z0-9]+$"))
                        context.AddFailure("UserName should only contains letters and numbers an \"_\".");
                }).MustAsync(BeUnique)
                .MinimumLength(3);

            RuleFor(x => x.Password)
                .NotEmpty().NotNull()
                .MinimumLength(5);
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().NotNull()
                .MinimumLength(5);

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.Password != x.ConfirmPassword)
                {
                    context.AddFailure(nameof(x.Password), "Password with ConfirmPassword dont match.");
                }
            });

        }

        private Task<bool> BeUnique(string arg1, CancellationToken arg2)
        {
            return _appDb.Users.AllAsync(x => x.UserName.ToLower() != arg1.ToLower());
        }
    }
}
