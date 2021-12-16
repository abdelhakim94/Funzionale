using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public static partial class Prelude
    {
        public static RwsAsync<MonoidW, Env, W, S, T> rwsAsync<MonoidW, Env, W, S, T>([DisallowNull][NotNull] T value)
            where MonoidW : struct, Monoid<W> => new((env, state) => Task.FromResult((value, default(MonoidW).Empty(), state)));

        public static RwsAsync<MonoidW, Env, W, S, T> rwsAsync<MonoidW, Env, W, S, T>(
            [DisallowNull][NotNull] T value, [DisallowNull][NotNull] W output) where MonoidW : struct, Monoid<W> =>
                new((env, state) => Task.FromResult((value, output, state)));

        public static RwsAsync<MonoidW, Env, W, S, T> rwsAsync<MonoidW, Env, W, S, T>(Func<Env, S, Task<(T value, W output, S state)>> f)
            where MonoidW : struct, Monoid<W> => f;
    }

    public readonly struct RwsAsync<MonoidW, Env, W, S, T> where MonoidW : struct, Monoid<W>
    {
        internal readonly Func<Env, S, Task<(T value, W output, S state)>> func;

        internal RwsAsync([DisallowNull][NotNull] Func<Env, S, Task<(T value, W output, S state)>> f) =>
            func = f ?? throw new ArgumentNullException(nameof(f), "Cannot initialize an Rws with an empty delegate.");

        public static implicit operator RwsAsync<MonoidW, Env, W, S, T>(Func<Env, S, Task<(T value, W output, S state)>> f) => new(f);

        public async Task<(Exceptional<T> value, W output, S state)> Run(Env env, S state)
        {
            try
            {
                if (state is null)
                    throw new ArgumentNullException(nameof(state), "The initiale state provided to the Rws monad was null. Consider using an Option instead.");

                var (value, output, newState) = await func(env, state);

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
