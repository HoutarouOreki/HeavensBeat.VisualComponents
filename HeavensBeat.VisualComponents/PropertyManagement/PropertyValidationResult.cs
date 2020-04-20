namespace HeavensBeat.VisualComponents.PropertyManagement
{
    public class PropertyValidationResult
    {
        public ValidationResult Result { get; }
        public string? Message { get; }

        public PropertyValidationResult(ValidationResult result, string? message = null)
        {
            Result = result;
            Message = message;
        }
    }

    public enum ValidationResult
    {
        Ok = 0,
        Warning = 1,
        Error = 2
    }
}