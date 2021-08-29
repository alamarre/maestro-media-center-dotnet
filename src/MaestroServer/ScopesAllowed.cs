using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MaestroServer
{
    public class ScopesAllowedAttribute : TypeFilterAttribute
    {
        public ScopesAllowedAttribute(params string[] acceptedScopes)
            : base(typeof(ScopesAllowedFilter))
        {
            Arguments = new object[] { acceptedScopes };
            IsReusable = true;
        }
    }

    public class ScopesAllowedFilter : IAuthorizationFilter
    {
        private readonly string[] acceptedScopes;

        public ScopesAllowedFilter(string[] acceptedScopes)
        {
            this.acceptedScopes = acceptedScopes;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var scope = context.HttpContext?.User?.FindFirst("scope");

            if (scope != null)
            {
                string[] scopes = scope.Value.Split(" ");
                if(scopes.Any(s => acceptedScopes.Contains(s))) {
                    return;
                }
            }

            context.Result = new ForbidResult();
        }
    }

}
