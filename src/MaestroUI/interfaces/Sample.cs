using System;
namespace MaestroUI.interfaces
{
    public class Sample : ISample
    {
        public Sample()
        {
        }

        string ISample.Name => "Sample";
    }
}
