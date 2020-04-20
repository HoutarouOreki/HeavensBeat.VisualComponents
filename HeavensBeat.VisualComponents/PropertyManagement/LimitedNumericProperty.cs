using System;

namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public class LimitedNumericProperty<T> : Property<T> where T : struct, IComparable<T>, IConvertible, IEquatable<T>
    {
        public T Min { get; }
        public T Max { get; }

        public LimitedNumericProperty(string name, string description, T min, T max, Func<T> getter, Action<T> setter, Func<T, PropertyValidationResult>? validator = null) : base(name, description, getter, setter, validator)
        {
            Min = min;
            Max = max;
        }
    }
}
