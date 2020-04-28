using System;

namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public class EnumProperty<T> : Property<T> where T : Enum
    {
        public EnumProperty(string name, string description, Func<T> getter, Action<T> setter, Func<T, PropertyValidationResult>? validator = null) : base(name, description, getter, setter, validator)
        {
        }
    }
}
