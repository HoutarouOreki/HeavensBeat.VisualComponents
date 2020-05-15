using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public class StringPropertyEditor : PropertyEditor<string>
    {
        public StringPropertyEditor(StringProperty property) : base(property) { }

        protected override Bindable<string> CreateCurrent() => new Bindable<string>("");

        protected override Drawable CreateMainContent() => new HBTextBox
        {
            Current = Current,
            RelativeSizeAxes = Axes.X,
            Height = 30
        };
    }
}
