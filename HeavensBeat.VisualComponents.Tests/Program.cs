using osu.Framework;

namespace HeavensBeat.VisualComponents.Tests
{
    internal class Program
    {
        private static void Main()
        {
            using var host = Host.GetSuitableHost("HeavensBeat.VisualComponents.Tests");
            host.Run(new TestBrowserContainer());
        }
    }
}
