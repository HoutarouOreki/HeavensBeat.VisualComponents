using HeavensBeat.VisualComponents.PropertyManagement;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace HeavensBeat.VisualComponents.Tests.Visual
{
    public class TestScenePropertyManagement : TestScene
    {
        public override IReadOnlyList<Type> RequiredTypes => new List<Type>
        {
            typeof(PropertyEditor<int>),
            typeof(LimitedNumericPropertyEditor<int>),
            typeof(Property<int>),
            typeof(LimitedNumericProperty<int>),
            typeof(PropertyEditor<TestEnum>),
            typeof(EnumPropertyEditor<TestEnum>),
            typeof(Property<TestEnum>),
            typeof(EnumProperty<TestEnum>)
        };

        private TestEnum theEnum = 0;
        private TestEnum2 secondEnum;
        private int integer = 0;
        private string text = "sdas";
        private readonly FillFlowContainer<PropertyEditor> flow;
        private readonly TextFlowContainer infoText;

        public TestScenePropertyManagement()
        {
            Add(new FillFlowContainer
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    flow = new FillFlowContainer<PropertyEditor>
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Children = new PropertyEditor[]
                        {
                            new LimitedNumericPropertyEditor<int>(new LimitedNumericProperty<int>("Limited integer", "A whole number with minimum and maximum values (-10, 50). Can't be 42.", -10, 50, () => integer, v => integer = v, ValidateIntegerNot42)) { Padding = new MarginPadding(12), Width = 400 },
                            new EnumPropertyEditor<TestEnum>(new EnumProperty<TestEnum>("Test enum property 1", "We're testing enumerable properties here. Selecting one results in an error", () => theEnum, v => theEnum = v, ValidateEnumNotOne)) { Padding = new MarginPadding(12), Width = 400 },
                            new EnumPropertyEditor<TestEnum2>(new EnumProperty<TestEnum2>("Test enum property 2", "More, the searchbox should be shown now. Selecting the last option results in a warning.", () => secondEnum, v => secondEnum = v, ValidateNotJ)) { Padding = new MarginPadding(12), Width = 400 },
                            new StringPropertyEditor(new StringProperty("Text property", "A string of letters. Length between 3 and 9 inclusive, can only contain either numbers or letters, but not both. Allowable length should be displayed in the future", () => text, v => text = v, ValidateText)) { Padding = new MarginPadding(12), Width = 400 },
                        }
                    },
                    infoText = new TextFlowContainer
                    {
                        AutoSizeAxes = Axes.Both
                    }
                }
            });
            AddSliderStep("Spacing+padding X", 0, 20f, 0, v => SetSpacing(v, true));
            AddSliderStep("Spacing+padding Y", 0, 20f, 0, v => SetSpacing(v, false));
            AddToggleStep("Toggle autosave", ToggleAutosave);
            AddStep("Save properties", Save);
        }

        private PropertyValidationResult ValidateText(string arg)
        {
            var s = "";
            if (arg.Length < 3 || arg.Length > 9)
                s += "Length should be from 3 to 9 characters. ";
            if (Regex.IsMatch(arg, @"[a-zA-Z]") && Regex.IsMatch(arg, @"\d"))
                s += "Cannot contain both letters and digits. ";
            if (arg.Length > 0 && !Regex.IsMatch(arg, @"^(\d|[a-zA-Z])*$"))
                s += "Must contain either letters xor digits.";
            return new PropertyValidationResult(s.Length > 0 ? ValidationResult.Error : ValidationResult.Ok, !string.IsNullOrWhiteSpace(s) ? s : null);
        }

        private void ToggleAutosave(bool obj)
        {
            foreach (var child in flow.Children)
                child.AutoSave = obj;
        }

        private void Save()
        {
            infoText.Text = "";
            foreach (var child in flow.Children)
            {
                if (!child.Save())
                    infoText.AddParagraph($"Couldn't save {child.PropertyName}");
            }
        }

        private void SetSpacing(float value, bool x)
        {
            flow.Spacing = new osuTK.Vector2(x ? value : flow.Spacing.X, x ? flow.Spacing.Y : value);
            foreach (var child in flow.Children)
            {
                child.Spacing = new osuTK.Vector2(x ? value : flow.Spacing.X, x ? flow.Spacing.Y : value);
                child.Padding = new MarginPadding { Horizontal = x ? value : child.Padding.Left, Vertical = x ? child.Padding.Top : value };
            }
        }

        private PropertyValidationResult ValidateIntegerNot42(int arg)
        {
            if (arg == 42)
                return new PropertyValidationResult(ValidationResult.Error, "The integer can't be 42.");
            else if (arg >= 41 && arg <= 43)
                return new PropertyValidationResult(ValidationResult.Warning, "Dangerously close to 42...");
            return new PropertyValidationResult(ValidationResult.Ok);
        }

        private PropertyValidationResult ValidateEnumNotOne(TestEnum arg)
        {
            if (arg == TestEnum.One)
                return new PropertyValidationResult(ValidationResult.Error, "The enum can't be \"one\".");
            return new PropertyValidationResult(ValidationResult.Ok);
        }

        private PropertyValidationResult ValidateNotJ(TestEnum2 arg)
        {
            if (arg == TestEnum2.OptionJ)
                return new PropertyValidationResult(ValidationResult.Warning, "You chose J.");
            return new PropertyValidationResult(ValidationResult.Ok);
        }

        private enum TestEnum
        {
            One,
            Two,
            Three
        }

        private enum TestEnum2
        {
            OptionA,
            OptionB,
            OptionC,
            OptionD,
            OptionE,
            OptionF,
            OptionG,
            OptionH,
            OptionI,
            OptionJ
        }
    }
}
