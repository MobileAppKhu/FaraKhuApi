using System;
using System.Linq.Expressions;
using Application.Resources;
using Microsoft.Extensions.Localization;

namespace Application.Utilities
{
    public static class LocalizerGetString
    {
        public static string GetString(this IStringLocalizer<SharedResource> localizer, Expression<Func<string>> expression)
        {
            var memberName = expression.Body switch
            {
                MemberExpression m =>
                    m.Member.Name,
                UnaryExpression u when u.Operand is MemberExpression m =>
                    m.Member.Name,
                _ =>
                    throw new NotImplementedException(expression.GetType().ToString())
            };
            return localizer[memberName];
        }
    }
}