using System;
namespace MaestroMediaCenter.Common
{
    public class UserProvider : IUserProvider
    {
        public UserProvider()
        {
        }

        string IUserProvider.User => "ahoy hoy";
    }
}
