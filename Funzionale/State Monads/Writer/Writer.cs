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
    }

    /// <summary>
    /// A monad that aggregates some result along the computations
    /// </summary>
    public readonly struct Writer<MonoidW, W, T> where MonoidW : struct, Monoid<W>
    {
        internal readonly Func<(T value, W output)> func;

        internal Writer([DisallowNull][NotNull] Func<(T valu, W output)> f) =>
            func = f ?? throw new ArgumentNullException(nameof(f), "Cannot initialize a writer with a null delegate");

        public static implicit operator Writer<MonoidW, W, T>([DisallowNull][NotNull] Func<(T value, W output)> f) => new(f);

        public (Exceptional<T> value, W output) Run()
        {
            try
            {
                var (value, output) = func();
                return (success(value!), output);
            }
            catch (Exception ex)
            {
                return (failure<T>(ex), default(MonoidW).Empty());
            }
        }
    }
}
