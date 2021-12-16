using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;
    using Validation;

    public static partial class Prelude
    {
        public static Validation<T> valid<T>([DisallowNull][NotNull] T t) => new(t);

        public static Invalid invalid([DisallowNull][NotNull] IEnumerable<Error> errors) => new(errors);
        public static Invalid invalid([DisallowNull][NotNull] params Error[] errors) => new(errors);

        public static Validation<T> invalid<T>([DisallowNull][NotNull] IEnumerable<Error> errors) => invalid(errors);
        public static Validation<T> invalid<T>([DisallowNull][NotNull] params Error[] errors) => invalid(errors);
    }

    namespace Validation
    {
        public readonly struct Invalid
        {
            internal readonly IEnumerable<Error> Errors;
            internal Invalid([DisallowNull][NotNull] IEnumerable<Error> errors) => Errors = errors
                ?? throw new ArgumentNullException(nameof(errors), "Cannot initialize the invalid state with a null collection of errors. Consider providing an empty Collection.");
        }
    }

    public readonly struct Validation<T>
    {
        private readonly IEnumerable<Error> errors;
        private readonly T? value;

        [MemberNotNullWhen(true, nameof(value))]
        private bool isValid { get; }

        internal Validation([DisallowNull][NotNull] T t)
        {

            value = t ?? throw new ArgumentNullException(nameof(t), "Cannot initialize a Validation in the valid state with a null value. Consider using an Option instead.");
            errors = Enumerable.Empty<Error>();
            isValid = true;
        }

        internal Validation([DisallowNull][NotNull] IEnumerable<Error> errors)
        {
            this.errors = errors ?? throw new ArgumentNullException(nameof(errors), "Cannot initialize a Validation in the invalid state with a null collection of errors. Consider providing an empty Collection.");
            value = default;
            isValid = false;
        }

        public static implicit operator Validation<T>(Invalid invalid) => new(invalid.Errors);
        public static implicit operator Validation<T>(Error error) => invalid(error);
        public static implicit operator Validation<T>([DisallowNull][NotNull] T t) => new(t);

        public static bool operator true(Validation<T> @this) => @this.isValid;
        public static bool operator false(Validation<T> @this) => !@this.isValid;

        public R Match<R>(Func<IEnumerable<Error>, R> Invalid, Func<T, R> Valid) => isValid ? Valid(value) : Invalid(errors);
    }
}