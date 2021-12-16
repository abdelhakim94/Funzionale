using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    public static partial class Prelude
    {
        public static Task<T> async<T>([DisallowNull][NotNull] T t) => Task.FromResult(t);
    }

    public static class TaskExtensions
    {
        // Functor

        public static async Task<R> Map<T, R>(this Task<T> @this, Func<T, R> map) =>
            map(await @this.ConfigureAwait(false));

        public static async Task<T> Map<T>(this Task @this, Func<T> map)
        {
            await @this.ConfigureAwait(false);
            return map();
        }

        public static Task<R> Map<T, R>(this Task<T> @this, Func<Exception, R> Faulted, Func<T, R> Completed) =>
            @this.ContinueWith(t => t.IsCompletedSuccessfully ? Completed(t.Result) : Faulted(t.Exception!));

        public static Task<T> Map<T>(this Task @this, Func<Exception, T> Faulted, Func<T> Completed) =>
            @this.ContinueWith(t => t.IsCompletedSuccessfully ? Completed() : Faulted(t.Exception!));

        public static Task<Func<T2, R>> Map<T1, T2, R>(this Task<T1> @this, Func<T1, T2, R> f) =>
            @this.Map(f.Curry());

        public static Task<Func<T2, T3, R>> Map<T1, T2, T3, R>(this Task<T1> @this, Func<T1, T2, T3, R> f) =>
            @this.Map(f.CurryFirst());

        public static Task<Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R>(this Task<T1> @this, Func<T1, T2, T3, T4, R> f) =>
            @this.Map(f.CurryFirst());

        public static Task<Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R>(this Task<T1> @this, Func<T1, T2, T3, T4, T5, R> f) =>
            @this.Map(f.CurryFirst());

        // Monad

        /// <summary>
        /// Tasks (@this and the task returned by bind) will not run in parallel. Use Apply
        /// if you want them to run in parallel.
        /// Use the monadic flow if you need the result of the first task to be able to start
        /// the second task. Use the Applicative flow otherwise.
        /// </summary>
        public static async Task<R> Bind<T, R>(this Task<T> @this, Func<T, Task<R>> bind) =>
            await bind(await @this.ConfigureAwait(false)).ConfigureAwait(false);

        // Applicative

        /// <summary>
        /// Tasks will run in parallel. Use the applicative flow if a function needs
        /// the result of many tasks to perform the computation. This way, tasks are
        /// started in parallel and no task depends on the result of the other.
        /// </summary>
        public static async Task<R> Apply<T, R>(this Task<Func<T, R>> @this, Task<T> arg) =>
            (await @this.ConfigureAwait(false))(await arg.ConfigureAwait(false));

        public static Task<Func<T2, R>> Apply<T1, T2, R>(this Task<Func<T1, T2, R>> @this, Task<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Task<Func<T2, T3, R>> Apply<T1, T2, T3, R>(this Task<Func<T1, T2, T3, R>> @this, Task<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Task<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>(this Task<Func<T1, T2, T3, T4, R>> @this, Task<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Task<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>(this Task<Func<T1, T2, T3, T4, T5, R>> @this, Task<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static Task<Unit> ForEach<T>(this Task<T> @this, Action<T> continuation) =>
            @this.ContinueWith(t => continuation.ToFunc()(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);

        public static Task<T> Recover<T>(this Task<T> @this, Func<Exception, T> fallback) =>
            @this.ContinueWith(t => t.IsCompletedSuccessfully ? t.Result : fallback(t.Exception!));

        public static Task<T> Recover<T>(this Task<T> @this, Func<Exception, Task<T>> fallback) =>
            @this.ContinueWith(t => t.IsCompletedSuccessfully ? Task.FromResult(t.Result) : fallback(t.Exception!)).Unwrap();

        // TODO: Traversable

        // LINQ

        public static Task<R> Select<T, R>(this Task<T> @this, Func<T, R> map) => @this.Map(map);

        public static Task<R> SelectMany<T, R>(this Task<T> @this, Func<T, Task<R>> bind) => @this.Bind(bind);

        public static Task<S> SelectMany<T, R, S>(this Task<T> @this, Func<T, Task<R>> bind, Func<T, R, S> project) =>
            @this.Bind(t => bind(t).Map(r => project(t, r)));

        public static async Task<T> Where<T>(this Task<T> @this, Func<T, bool> predicate)
        {
            var result = await @this.ConfigureAwait(false);
            return predicate(result) ? result : throw new OperationCanceledException();
        }
    }
}
