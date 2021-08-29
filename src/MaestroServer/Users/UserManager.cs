using System;
using MaestroServer.Middleware;
using Microsoft.AspNetCore.Http;

namespace MaestroServer.Users
{
    public class UserManager : IUserManager
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserManager(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        object IUserManager.GetCurrentUser 
        {
            get
            {
                object email = null;
                this.httpContextAccessor?.HttpContext?.Items?.TryGetValue(UserMiddleware.USER_KEY, out email);
                return email;
            }
        }
    }
}
