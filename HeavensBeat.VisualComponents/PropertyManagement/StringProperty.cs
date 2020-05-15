using System;
using System.Collections.Generic;
using System.Text;

namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public class StringProperty : Property<string>
    {
        public StringProperty(string name, string description, Func<string> getter, Action<string> setter, Func<string, PropertyValidationResult>? validator = null) : base(name, description, getter, setter, validator)
        {
        }
    }
}
