using System;

namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public abstract class Property<T>
    {
        public string Name { get; }
        public string Description { get; }
        public Func<T> Getter { get; }
        public Action<T> Setter { get; }
        public Func<T, PropertyValidationResult>? Validator { get; }

        protected Property(string name, string description, Func<T> getter, Action<T> setter, Func<T, PropertyValidationResult>? validator = null)
        {
            Name = name;
            Description = description;
            Getter = getter;
            Setter = setter;
            Validator = validator;
        }
    }

    public enum PropertyState
    {
        Unknown = 0,
        Saved = 1,
        ReadyToSave = 2,
        Warning = 3,
        Error = 4,
    }
}
