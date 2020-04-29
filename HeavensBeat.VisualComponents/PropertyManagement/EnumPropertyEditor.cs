using osu.Framework.Bindables;
using osu.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public class EnumPropertyEditor<T> : PropertyEditor<T> where T : Enum
    {
        private readonly IReadOnlyDictionary<int, string> names;
        private readonly Bindable<string> bindableString;

        public EnumPropertyEditor(Property<T> property) : base(property)
        {
            names = GenerateNames();
            bindableString = new Bindable<string>(Enum.GetName(typeof(T), Current.Value) ?? throw new ArgumentNullException());
            bindableString.BindValueChanged(OnSelectionChanged);
        }

        protected override Bindable<T> CreateCurrent() => new Bindable<T>();

        protected override Drawable CreateMainContent() => new SearchSelector
        {
            RelativeSizeAxes = Axes.X,
            Height = 268,
            Current = bindableString,
            Options = names.Values
        };

        private void OnSelectionChanged(ValueChangedEvent<string> sc) => Current.Value = (T)Enum.Parse(typeof(T), sc.NewValue);

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
