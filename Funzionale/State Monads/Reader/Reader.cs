using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;
    public static partial class Prelude
    {
        public static Reader<Env, T> reader<Env, T>([DisallowNull][NotNull] Func<Env, T> func) where Env : struct => func;

        public static Reader<Env, T> reader<Env, T>([DisallowNull][NotNull] T value) where Env : struct => new(env => value);
    }

    /// <summary>
    /// Represents an IO monad with an injectable environment to make it unit testable.
    /// </summary>
    public readonly struct Reader<Env, T> where Env : struct
    {
        internal readonly Func<Env, T> func;

        internal Reader([DisallowNull][NotNull] Func<Env, T> func) =>
            this.func = func ?? throw new ArgumentNullException(nameof(func), "Cannot initialize a Reader with a null delegate function.");

        public static implicit operator Reader<Env, T>(Func<Env, T> func) => new(func);

        public Exceptional<T> Run(Env env)
        {
            try
            {
                return success(func(env) ?? throw new ArgumentNullException(nameof(func), "The function provided to the Reader returned null. Consider using an Option."));
            }
            catch (Exception ex)
            {
                return failure<T>(ex);
            }
        }
    }
}
