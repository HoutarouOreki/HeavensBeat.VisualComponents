using HeavensBeat.VisualComponents.PropertyManagement;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using System;
using System.Collections.Generic;

namespace HeavensBeat.VisualComponents.Tests.Visual.PropertyManagement
{
    public class TestSceneLimitedNumeric : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new List<Type>
        {
            typeof(PropertyEditor<int>),
            typeof(LimitedNumericPropertyEditor<int>),
            typeof(Property<int>),
            typeof(LimitedNumericProperty<int>)
        };

        private int integer = 0;
        private readonly LimitedNumericPropertyEditor<int> limNumPE;

        public TestSceneLimitedNumeric()
        {
            Add(new Container
            {
                Width = 400,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    limNumPE = new LimitedNumericPropertyEditor<int>(new LimitedNumericProperty<int>("Limited integer", "A whole number with minimum and maximum values (-10, 50). Can't be 42.", -10, 50, () => integer, v => integer = v, ValidateIntegerNot42)) { Padding = new MarginPadding(12) }
                }
            });
            AddSliderStep("Change spacing X", 0, 20f, 0, v => limNumPE.Spacing = new osuTK.Vector2(v, limNumPE.Spacing.Y));
            AddSliderStep("Change spacing Y", 0, 20f, 0, v => limNumPE.Spacing = new osuTK.Vector2(limNumPE.Spacing.X, v));
        }

        private PropertyValidationResult ValidateIntegerNot42(int arg)
        {
            if (arg == 42)
                return new PropertyValidationResult(ValidationResult.Error, "The integer can't be 42.");
            else if (arg >= 41 && arg <= 43)
                return new PropertyValidationResult(ValidationResult.Warning, "Dangerously close to 42...");
            return new PropertyValidationResult(ValidationResult.Ok, "All ok!");
        }
    }
}
