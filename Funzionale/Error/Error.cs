namespace Funzionale
{
    using static Prelude;

    public readonly struct Error
    {
        public Option<string> Message { get; init; }
        public Option<Exception> Exception { get; init; }
        public Option<Error> Inner => from e in inner select e.ToError();
        
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
}
