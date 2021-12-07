namespace Funzionale
{
    using static Prelude;
    public static partial class Prelude
    {
        public static Option<T> some<T>(T value) => new Option.Some<T>(value);
        public static Option.None none => default;
    }

    namespace Option
    {
        public struct Some<T>
        {
            internal T value;

            public Some(T value)
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value), "'Some' cannot wrap a null value. Use None instead.");
                this.value = value;
            }
        }

        public struct None { }
    }

    public struct Option<T>  : IEquatable<Option<T>>, IEquatable<Option.None>
    {
        readonly T value;
        readonly bool IsSome => value is not null;
        readonly bool IsNone => !IsSome;

        private Option(T value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            this.value = value;
        }

        public static implicit operator Option<T>(Option.None _) => new();
        public static implicit operator Option<T>(Option.Some<T> some) => new(some.value);
        public static implicit operator Option<T>(T value) => value is null ? none : some(value);

        public R Match<R>(Func<R> None, Func<T, R> Some) => IsSome ? Some(value) : None();

        public bool Equals(Option<T> other) => 
            this.IsSome == other.IsSome && (this.IsNone || value!.Equals(other.value));

        public bool Equals(Option.None _) => IsNone;

        public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);
        public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);

        public override bool Equals(object? obj) => obj is Option<T> other && this == other;

        public override int GetHashCode() => IsNone ? 0 : value!.GetHashCode();
    }
}
