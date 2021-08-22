using System;
namespace MaestroMediaCenter.Common
{
    public class SampleChain
    {
        private readonly IUserProvider userProvider;
        public SampleChain(IUserProvider userProvider)
        {
            this.userProvider = userProvider;
        }

        public string Test { get => userProvider.User; }
    }
}
