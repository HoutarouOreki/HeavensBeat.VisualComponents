using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public class EnumPropertyEditor<T> : PropertyEditor<T> where T : Enum
    {
        private readonly IReadOnlyDictionary<int, string> names;
        private readonly Bindable<string> bindableString;
        private SearchSelector? searchSelector;

        public EnumPropertyEditor(Property<T> property) : base(property)
        {
            names = GenerateNames();
            bindableString = new Bindable<string>(Enum.GetName(typeof(T), Current.Value) ?? throw new ArgumentNullException());
            bindableString.BindValueChanged(OnSelectionChanged);
        }

        protected override Bindable<T> CreateCurrent() => new Bindable<T>();

        protected override Drawable CreateMainContent() => searchSelector = new SearchSelector
        {
            RelativeSizeAxes = Axes.X,
            Spacing = Spacing,
            Current = bindableString,
            Options = names.Values
        };

        protected override void LoadComplete()
        {
            base.LoadComplete();
            SetSearchSelectorHeight();
        }

        protected override void OnSpacingChanged(Vector2 value)
        {
            base.OnSpacingChanged(value);
            if (searchSelector != null)
            {
                searchSelector.Spacing = value;
                SetSearchSelectorHeight();
            }
        }

        private void OnSelectionChanged(ValueChangedEvent<string> sc) => Current.Value = (T)Enum.Parse(typeof(T), sc.NewValue);

        private float SetSearchSelectorHeight()
        {
            if (searchSelector == null)
                throw new ArgumentNullException();
            var size = 0f;
            if (searchSelector.DeselectOption)
                size += SearchSelector.OPTION_SIZE;
            if (names.Count > searchSelector.OptionsWithoutSearch)
                size += Spacing.Y + SearchSelector.OPTION_SIZE;
            size += Math.Min(4, names.Count) * SearchSelector.OPTION_SIZE;
            searchSelector.Height = size;
            return size;
        }

        private Dictionary<int, string> GenerateNames()
        {
            var result = new Dictionary<int, string>();
            var values = Enum.GetValues(typeof(T)).Cast<int>().ToList();

            foreach (var item in values)
                result.Add(item, Enum.GetName(typeof(T), item) ?? "");
            return result;
        }
    }
}
