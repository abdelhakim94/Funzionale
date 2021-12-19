namespace Funzionale
{
    public static class Misc
    {
        public static R Transform<T, R>(this T @this, Func<T, R> transform) => transform(@this);

        public static T Tee<T>(this T @this, Action<T> action)
        {
            action(@this);
            return @this;
        }

        /// <summary>
        /// Wraps the "using" statement in a function to make it composable with other functions.
        /// </summary>
        public static R Using<TDisp, R>(TDisp disposable, Func<TDisp, R> f) where TDisp : IDisposable
        {
            using (disposable) return f(disposable);
        }

        /// <summary>
        /// Wraps the "using" statement in a function to make it composable with other functions.
        /// awaits the Task returned by f so that the disposable object does not dispose while
        /// the Task is running.
        public static async Task<R> Using<TDisp, R>(TDisp disposable, Func<TDisp, Task<R>> f) where TDisp : IDisposable
        {
            using (disposable) return await f(disposable);
        }

        /// <summary>
        /// Wraps the "using" statement in a function and makes it composable with other functions.
        /// </summary>
        public static async Task<R> UsingAsync<TAsyncDisp, R>(TAsyncDisp disposable, Func<TAsyncDisp, R> f) where TAsyncDisp : IAsyncDisposable
        {
            await using (disposable) return f(disposable);
        }

        /// <summary>
        /// Wraps the "using" statement in a function and makes it composable with other functions.
        /// awaits the Task returned by f so that the disposable object does not dispose while the
        /// Task is running.
        public static async Task<R> UsingAsync<TAsyncDisp, R>(TAsyncDisp disposable, Func<TAsyncDisp, Task<R>> f) where TAsyncDisp : IAsyncDisposable
        {
            await using (disposable) return await f(disposable);
        }
    }
}
