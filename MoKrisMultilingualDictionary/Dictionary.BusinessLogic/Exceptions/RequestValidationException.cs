namespace Dictionary.BusinessLogic.Exceptions
{
    public class RequestValidationException : Exception
    {
        public RequestValidationException(string name, string detailedErrorMessage) : base($"Request '{name}' validation failed. '{detailedErrorMessage}'")
        {
        }
    }
}
