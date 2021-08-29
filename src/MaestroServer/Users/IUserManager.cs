using System;
namespace MaestroServer.Users
{
    public interface IUserManager
    {
        object GetCurrentUser { get; }
    }
}
