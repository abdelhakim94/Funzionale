using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;
    public static partial class Prelude
    {
        public static ReaderAsync<Env, T> readerAsync<Env, T>([DisallowNull][NotNull] Func<Env, Task<T>> func) where Env : struct =>
            func;

        public static ReaderAsync<Env, T> readerAsync<Env, T>([DisallowNull][NotNull] T value) where Env : struct =>
            new(env => Task.FromResult(value));
    }

    /// <summary>
    /// Represents an asynchronous IO monad with an injectable environment to make it unit testable.
    /// </summary>
    public readonly struct ReaderAsync<Env, T> where Env : struct
    {
        internal readonly Func<Env, Task<T>> func;

        internal ReaderAsync([DisallowNull][NotNull] Func<Env, Task<T>> func) =>
            this.func = func ?? throw new ArgumentNullException(nameof(func), "Cannot initialize a Reader with a null delegate function.");

        public static implicit operator ReaderAsync<Env, T>(Func<Env, Task<T>> func) => new(func);

        public async Task<Exceptional<T>> Run(Env env)
        {
            try
            {
                return success(await func(env)
                    ?? throw new ArgumentNullException(nameof(func), "The function provided to the Reader returned null. Consider using an Option."));
            }
            catch (Exception ex)
            {
                return failure<T>(ex);
            }
        }
    }
}
