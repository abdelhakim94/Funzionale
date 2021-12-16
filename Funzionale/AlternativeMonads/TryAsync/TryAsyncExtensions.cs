namespace Funzionale
{
    public static class TryAsyncAsyncExtensions
    {
        // Functor

        public static TryAsync<R> Map<T, R>(this TryAsync<T> @this, Func<T, R> map) => new(() => @this.func().Map(map));

        public static TryAsync<Func<T2, R>> Map<T1, T2, R>(this TryAsync<T1> @this, Func<T1, T2, R> map) =>
            @this.Map(map.Curry());

        public static TryAsync<Func<T2, T3, R>> Map<T1, T2, T3, R>(this TryAsync<T1> @this, Func<T1, T2, T3, R> map) =>
            @this.Map(map.CurryFirst());

        public static TryAsync<Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R>(this TryAsync<T1> @this, Func<T1, T2, T3, T4, R> map) =>
            @this.Map(map.CurryFirst());

        public static TryAsync<Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R>(this TryAsync<T1> @this, Func<T1, T2, T3, T4, T5, R> map) =>
            @this.Map(map.CurryFirst());

        // Monad

        public static TryAsync<R> Bind<T, R>(this TryAsync<T> @this, Func<T, TryAsync<R>> bind) =>
            new(async () => await bind(await @this.func().ConfigureAwait(false)).func().ConfigureAwait(false));

        // Apply

        public static TryAsync<R> Apply<T, R>(this TryAsync<Func<T, R>> @this, TryAsync<T> arg) =>
            new(async () => (await @this.func().ConfigureAwait(false))(await arg.func().ConfigureAwait(false)));

        public static TryAsync<Func<T2, R>> Apply<T1, T2, R>(this TryAsync<Func<T1, T2, R>> @this, TryAsync<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static TryAsync<Func<T2, T3, R>> Apply<T1, T2, T3, R>(this TryAsync<Func<T1, T2, T3, R>> @this, TryAsync<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static TryAsync<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>(this TryAsync<Func<T1, T2, T3, T4, R>> @this, TryAsync<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static TryAsync<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>(this TryAsync<Func<T1, T2, T3, T4, T5, R>> @this, TryAsync<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static TryAsync<Unit> ForEach<T>(this TryAsync<T> @this, Action<T> action) => @this.Map(action.ToFunc());

        // LINQ

        public static TryAsync<R> Select<T, R>(this TryAsync<T> @this, Func<T, R> map) => @this.Map(map);

        public static TryAsync<R> SelectMany<T, R>(this TryAsync<T> @this, Func<T, TryAsync<R>> bind) => @this.Bind(bind);

        public static TryAsync<S> SelectMany<T, R, S>(this TryAsync<T> @this, Func<T, TryAsync<R>> bind, Func<T, R, S> project) =>
            @this.Bind(t => bind(t).Map(r => project(t, r)));
    }
}
