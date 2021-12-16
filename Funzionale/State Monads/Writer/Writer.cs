using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;
    public static partial class Prelude
    {
        public static Writer<MonoidW, W, T> writer<MonoidW, W, T>([DisallowNull][NotNull] T value)
            where MonoidW : struct, Monoid<W> =>
                new(() => (value, default(MonoidW).Empty()));

        public static Writer<MonoidW, W, T> writer<MonoidW, W, T>([DisallowNull][NotNull] T value, [DisallowNull][NotNull] W output)
            where MonoidW : struct, Monoid<W> =>
                new(() => (value, output));

        public static Writer<MonoidW, W, T> writer<MonoidW, W, T>([DisallowNull][NotNull] Func<(T value, W output)> f)
            where MonoidW : struct, Monoid<W> => f;
    }

    /// <summary>
    /// A monad that produces a value T and aggregates some other result W along the computation
    /// </summary>
    public readonly struct Writer<MonoidW, W, T> where MonoidW : struct, Monoid<W>
    {
        internal readonly Func<(T value, W output)> func;

        internal Writer([DisallowNull][NotNull] Func<(T value, W output)> f) =>
            func = f ?? throw new ArgumentNullException(nameof(f), "Cannot initialize a writer with a null delegate");

        public static implicit operator Writer<MonoidW, W, T>([DisallowNull][NotNull] Func<(T value, W output)> f) => new(f);

        public (Exceptional<T> value, W output) Run()
        {
            try
            {
                var (value, output) = func();

                if (value is null)
                    throw new ArgumentNullException(nameof(value), "The function provided to the Writer returned a null value. Consider using an Option instead.");

                if (output is null)
                    throw new ArgumentNullException(nameof(value), "The function provided to the Writer returned a null aggregate. Consider using an Option instead.");

                return (success(value), output);
            }
            catch (Exception ex)
            {
                return (failure<T>(ex), default(MonoidW).Empty());
            }
        }
    }
}
