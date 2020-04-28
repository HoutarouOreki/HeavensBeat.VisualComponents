using HeavensBeat.VisualComponents.PropertyManagement;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Testing;
using System;
using System.Collections.Generic;

namespace HeavensBeat.VisualComponents.Tests.Visual.PropertyManagement
{
    public class TestSceneEnum : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new List<Type>
        {
            typeof(PropertyEditor<TestEnum>),
            typeof(EnumPropertyEditor<TestEnum>),
            typeof(Property<TestEnum>),
            typeof(EnumProperty<TestEnum>)
        };

        private TestEnum theEnum = 0;
        private readonly EnumPropertyEditor<TestEnum> testEnumPE;

        public TestSceneEnum()
        {
            Add(new Container
            {
                Width = 400,
                AutoSizeAxes = Axes.Y,
                Children = new Drawable[]
                {
                    testEnumPE = new EnumPropertyEditor<TestEnum>(new EnumProperty<TestEnum>("Test enum property", "We're testing enumerable properties here.", () => theEnum, v => theEnum = v, ValidateEnumNotOne)) { Padding = new MarginPadding(12) }
                }
            });
            AddSliderStep("Change spacing X", 0, 20f, 0, v => testEnumPE.Spacing = new osuTK.Vector2(v, testEnumPE.Spacing.Y));
            AddSliderStep("Change spacing Y", 0, 20f, 0, v => testEnumPE.Spacing = new osuTK.Vector2(testEnumPE.Spacing.X, v));
        }

        private PropertyValidationResult ValidateEnumNotOne(TestEnum arg)
        {
            if (arg == TestEnum.One)
                return new PropertyValidationResult(ValidationResult.Error, "The enum can't be \"one\".");
            return new PropertyValidationResult(ValidationResult.Ok, "All ok!");
        }

        private enum TestEnum
        {
            One,
            Two,
            Three
        }
    }
}
