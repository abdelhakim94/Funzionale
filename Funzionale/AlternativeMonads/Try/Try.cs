using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public static partial class Prelude
    {
        public static Try<T> @try<T>([DisallowNull][NotNull] T value) => new(() => value);
        public static Try<T> @try<T>([DisallowNull][NotNull] Func<T> f) => f;
    }

    public readonly struct Try<T>
    {
        internal readonly Func<T> func;

        internal Try([DisallowNull][NotNull] Func<T> f) =>
            func = f ?? throw new ArgumentNullException(nameof(f), "Cannot initialize a Try with a null delegate.");

        public static implicit operator Try<T>(Func<T> f) => new(f);

        public Exceptional<T> Run()
        {
            try
            {
                return success(func() ?? throw new ArgumentNullException("The function provided to the Try monad returned null. Consider using an Option instead."));
            }
            catch (Exception ex)
            {
                return failure<T>(ex);
            }
        }
    }
}
