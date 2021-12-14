using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    public static partial class Prelude
    {
        public static Exceptional<T> success<T>([DisallowNull][NotNull] T t) => new(t);
        public static Exceptional<T> failure<T>([DisallowNull][NotNull] Exception ex) => new(ex);
    }

    public readonly struct Exceptional<T>
    {
        private readonly Exception? exception;
        private readonly T? value;

        [MemberNotNullWhen(true, nameof(value))]
        [MemberNotNullWhen(false, nameof(exception))]
        private bool isSuccess { get; }

        internal Exceptional([DisallowNull][NotNull] Exception ex)
        {
            exception = ex ?? throw new ArgumentNullException(nameof(ex), "Cannot initialize an Exceptional in the exception state with a null exception.");
            value = default;
            isSuccess = false;
        }

        internal Exceptional([DisallowNull][NotNull] T t)
        {
            value = t ?? throw new ArgumentNullException(nameof(t), "Cannot initialize an Exceptional in the success state with a null value.");
            isSuccess = true;
            exception = default;
        }

        public static implicit operator Exceptional<T>([DisallowNull][NotNull] Exception ex) => new(ex);
        public static implicit operator Exceptional<T>([DisallowNull][NotNull] T t) => new(t);

        public static bool operator true(Exceptional<T> @this) => @this.isSuccess;
        public static bool operator false(Exceptional<T> @this) => !@this.isSuccess;

        public R Match<R>(Func<Exception, R> Exception, Func<T, R> Success) => isSuccess ? Success(value) : Exception(exception);
    }
}
