using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public static partial class Prelude
    {
        public static TryAsync<T> tryAsync<T>([DisallowNull][NotNull] T value) => new(() => Task.FromResult(value));
        public static TryAsync<T> tryAsync<T>([DisallowNull][NotNull] Func<Task<T>> f) => f;
    }

    public readonly struct TryAsync<T>
    {
        internal readonly Func<Task<T>> func;

        internal TryAsync([DisallowNull][NotNull] Func<Task<T>> f) =>
            func = f ?? throw new ArgumentNullException(nameof(f), "Cannot initialize a Try with a null delegate.");

        public static implicit operator TryAsync<T>(Func<Task<T>> f) => new(f);

        public async Task<Exceptional<T>> Run()
        {
            try
            {
                return success(await func() ?? throw new ArgumentNullException("The function provided to the Try monad returned null. Consider using an Option instead."));
            }
            catch (Exception ex)
            {
                return failure<T>(ex);
            }
        }
    }
}
