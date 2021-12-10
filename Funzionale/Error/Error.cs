namespace Funzionale
{
    using static Prelude;

    public readonly struct Error
    {
        public Option<string> Message { get; init; }
        public Option<Exception> Exception { get; init; }
        public Option<Error> Inner
        {
            get => from i in inner select i.ToError();
            init => inner = from e in value select e.ToInnerError();
        }

        internal Option<InnerError> inner { get; init; }

        internal InnerError ToInnerError() => new()
        {
            Message = Message,
            Exception = Exception,
            Inner = inner,
        };

        public static implicit operator Error(string message) => new() { Message = some(message) };
        public static implicit operator Error(Exception exception) => new() { Exception = some(exception) };

        public static implicit operator Error((string message, Exception exception) info) => new()
        {
            Message = some(info.message),
            Exception = some(info.exception),
        };

        public static implicit operator Error((string message, Error inner) info) => new()
        {
            Message = some(info.message),
            inner = some(info.inner.ToInnerError()),
        };
    }

    /// <summary>
    /// This is intended to be used to avoid cycle errors in the Error
    /// struct when nesting errors.
    /// </summary>
    internal sealed class InnerError
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
