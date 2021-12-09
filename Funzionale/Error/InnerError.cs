namespace Funzionale
{
    /// <summary>
    /// This is intended to be used to avoid cycle errors in the Error
    /// struct when nesting errors.
    /// </summary>
    internal class InnerError
    {
        public Option<string> Message { get; init; }
        public Option<Exception> Exception { get; init; }
        public Option<InnerError> Inner { get; init; }

        public Error ToError() => new()
        {
            Message = Message,
            Exception = Exception,
            inner = Inner,
        };
    }
}
