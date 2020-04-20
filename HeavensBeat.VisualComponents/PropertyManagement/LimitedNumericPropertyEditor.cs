using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osuTK;
using System;

namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public class LimitedNumericPropertyEditor<T> : PropertyEditor<T> where T : struct, IComparable<T>, IConvertible, IEquatable<T>
    {
        private readonly Bindable<string> currentText = new Bindable<string>("");
        private GridItemsContainer? gridFlow;

        public LimitedNumericPropertyEditor(LimitedNumericProperty<T> property) : base(property) => currentText.BindValueChanged(OnTextChanged, true);

        public BindableNumber<T>? BindableNumber { get; private set; }

        protected override Bindable<T> CreateCurrent() => BindableNumber = new BindableNumber<T>
        {
            MinValue = (Property as LimitedNumericProperty<T>)?.Min ?? throw new Exception(),
            MaxValue = (Property as LimitedNumericProperty<T>)?.Max ?? throw new Exception()
        };

        protected override Drawable CreateMainContent() => gridFlow = new GridItemsContainer(GridDirection.Horizontal)
        {
            RelativeSizeAxes = Axes.X,
            Height = 30,
            Children = new GridItem[]
            {
                new GridItem(new HBSliderBar<T>
                {
                    RelativeSizeAxes = Axes.Both,
                    Current = BindableNumber,
                }),
                new GridItem(60, false, new HBTextBox
                {
                    RelativeSizeAxes = Axes.Both,
                    Current = currentText
                })
            }
        };

        protected override void OnSpacingChanged(Vector2 value)
        {
            if (gridFlow != null)
            {
                gridFlow.Spacing = value;
            }
            else
                OnLoadComplete += SetSpacing;
            base.OnSpacingChanged(value);
        }

        protected override void OnCurrentChanged(ValueChangedEvent<T> change)
        {
            if (currentText is null)
                throw new Exception();
            currentText.Value = change.NewValue.ToString() ?? "";
        }

        private void SetSpacing(Drawable obj)
        {
            OnLoadComplete -= SetSpacing;
            if (gridFlow != null)
                gridFlow.Spacing = Spacing;
        }

        private void OnTextChanged(ValueChangedEvent<string> vc)
        {
            if (BindableNumber is null)
                throw new Exception("BindableNumber was null");
            if (string.IsNullOrWhiteSpace(vc.NewValue))
            {
                Current.Value = default;
                return;
            }

            try
            {
                var value = GetValue(vc.NewValue);
                if (value.CompareTo(BindableNumber.MinValue) < 0 || value.CompareTo(BindableNumber.MaxValue) > 0)
                    TriggerError("Entered number is out of range.");
                else if (value.Equals(Current.Value))
                    Current.TriggerChange();
                else
                    Current.Value = value;
            }
            catch (Exception) { TriggerError("Uncastable to a number."); };
        }

        private T GetValue(string value) => (T)Convert.ChangeType(value, typeof(T));
    }
}
