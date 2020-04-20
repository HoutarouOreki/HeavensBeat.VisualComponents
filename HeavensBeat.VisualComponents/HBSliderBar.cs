using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using System;

namespace HeavensBeat.VisualComponents
{
    public class HBSliderBar<T> : BasicSliderBar<T> where T : struct, IComparable<T>, IConvertible, IEquatable<T>
    {
        public Func<T, string> TextGeneration = t => t.ToString() ?? "";

        private readonly SpriteText valueText;

        public HBSliderBar()
        {
            KeyboardStep = 0.01f;
            AddInternal(new Container
            {
                AutoSizeAxes = Axes.Both,
                Depth = -1,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new Box
                    {
                        Colour = ColoursHB.MediumBlack,
                        RelativeSizeAxes = Axes.Both,
                        BypassAutoSizeAxes = Axes.Both,
                    },
                    valueText = new SpriteText
                    {
                        Font = new FontUsage("OpenSans-Bold"),
                        Margin = new MarginPadding { Horizontal = 4 }
                    }
                }
            });
            Current.BindValueChanged(vc => valueText.Text = TextGeneration(vc.NewValue), true);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            FinishTransforms(true);
        }
    }
}
