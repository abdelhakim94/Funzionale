namespace Funzionale
{
    using System.Diagnostics.CodeAnalysis;
    using static Prelude;
    public static partial class Prelude
    {
        /// <summary>
        /// If you explicitely use 'some' to lift a value in an 'Option', then you are 
        /// supposed to be sure that the value is not null. If you are not sure, consider
        /// either checking for null-state or assigning the value directly to an 'Option'
        /// variable. The implicit conversion between your value and the 'Option' takes
        /// care of checking the null-state
        /// </summary>
        /// <returns>An 'Option' in the 'Some' state wrapping your value</returns>
        /// <exception cref="ArgumentNullException">The provided value was null</exception>
        public static Option<T> some<T>([DisallowNull] T value) =>
            value ?? throw new ArgumentNullException(nameof(value), "'Some' cannot wrap a null value. Use None instead.");

        public static Option.None none => default;
    }

    namespace Option
    {
        public struct None { }
    }

    public struct Option<T> : IEquatable<Option<T>>, IEquatable<Option.None>
    {
        readonly T value;

        [MemberNotNullWhen(true, nameof(value))]
        readonly bool IsSome => value is not null;

        [MemberNotNullWhen(false, nameof(value))]
        readonly bool IsNone => !IsSome;

        public static implicit operator Option<T>(Option.None _) => default;
        public static implicit operator Option<T>(T value) => value is null ? none : value;

        public R Match<R>(Func<R> None, Func<T, R> Some) => IsSome ? Some(value) : None();

        public static bool operator true(Option<T> @this) => @this.IsSome;
        public static bool operator false(Option<T> @this) => @this.IsNone;
        public static Option<T> operator |(Option<T> l, Option<T> r) => l.IsSome ? l : r;


        public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
        public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);

        public bool Equals(Option<T> other) =>
            this.IsSome == other.IsSome && (this.IsNone || value.Equals(other.value));

        public bool Equals(Option.None _) => IsNone;

        public override bool Equals(object? obj) => obj is Option<T> other && this == other;

        public override int GetHashCode() => IsNone ? 0 : value.GetHashCode();
    }
}
