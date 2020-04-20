using osu.Framework;
using osu.Framework.Testing;

namespace HeavensBeat.VisualComponents.Tests
{
    public class TestBrowserContainer : Game
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();
            Add(new TestBrowser(""));
        }
    }
}
