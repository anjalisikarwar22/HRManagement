namespace HRManagement.API.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            : base("Validation failed.")
        {
            Errors = errors;
        }

        public ValidationException(string error)
            : this(new[] { error }) { }
    }
}
