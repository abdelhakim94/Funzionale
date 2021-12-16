using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public static partial class Prelude
    {
    }

    public readonly struct Rws<MonoidW, Env, W, S, T> where MonoidW : struct, Monoid<W>
    {
        internal readonly Func<Env, S, (T value, W output, S state)> func;

        internal Rws([DisallowNull][NotNull] Func<Env, S, (T value, W output, S state)> f) =>
            func = f ?? throw new ArgumentNullException(nameof(f), "Cannot initialize an Rws with an empty delegate.");

        public static implicit operator Rws<MonoidW, Env, W, S, T>(Func<Env, S, (T value, W output, S state)> f) => new(f);

        public (Exceptional<T> value, W output, S state) Run(Env env, S state)
        {
            try
            {
                if (state is null)
                    throw new ArgumentNullException(nameof(state), "The initiale state provided to the Rws monad was null. Consider using an Option instead.");

                var (value, output, newState) = func(env, state);

                if (value is null)
                    throw new ArgumentNullException(nameof(func), "The function provided to the Rws returned null. Consider using an Option.");

                if (output is null)
                    throw new ArgumentNullException(nameof(value), "The function provided to the Writer returned a null aggregate. Consider using an Option instead.");

                if (newState is null)
                    throw new ArgumentNullException(nameof(value), "The function provided to the Writer returned a null aggregate. Consider using an Option instead.");

                return (success(value), output, newState);
            }
            catch (Exception ex)
            {
                return (failure<T>(ex), default(MonoidW).Empty(), state);
            }
        }
    }
}
