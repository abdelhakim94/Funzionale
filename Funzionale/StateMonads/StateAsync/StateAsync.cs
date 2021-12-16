using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;
    public static partial class Prelude
    {
        public static StateAsync<S, T> stateAsync<S, T>([DisallowNull][NotNull] T value) =>
            new(state => Task.FromResult((value, state)));

        public static StateAsync<S, T> stateAsync<S, T>([DisallowNull][NotNull] Func<S, Task<(T value, S state)>> f) => f;
    }

    /// <summary>
    /// A monad that produces a value T and changes state along the computation
    /// </summary>
    public readonly struct StateAsync<S, T>
    {
        internal readonly Func<S, Task<(T value, S state)>> func;

        internal StateAsync([DisallowNull][NotNull] Func<S, Task<(T value, S state)>> f) =>
            func = f ?? throw new ArgumentNullException(nameof(f), "Cannot initialize a State monad with an empty delegate");

        public static implicit operator StateAsync<S, T>([DisallowNull][NotNull] Func<S, Task<(T value, S state)>> f) => new(f);

        public async Task<(Exceptional<T> value, S state)> Run(S state)
        {
            try
            {
                if (state is null)
                    throw new ArgumentNullException(nameof(state), "The initiale state provided to the State monad was null. Consider using an Option instead.");

                var (value, newState) = await func(state).ConfigureAwait(false);

                if (value is null)
                    throw new ArgumentNullException(nameof(value), "The function provided to the Writer returned a null value. Consider using an Option instead.");

                if (newState is null)
                    throw new ArgumentNullException(nameof(value), "The function provided to the Writer returned a null aggregate. Consider using an Option instead.");

                return (success(value!), newState);
            }
            catch (Exception ex)
            {
                return (failure<T>(ex), state);
            }
        }
    }
}
