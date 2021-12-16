namespace Funzionale
{
    public static class TryExtensions
    {
        // Functor

        public static Try<R> Map<T, R>(this Try<T> @this, Func<T, R> map) => new(() => map(@this.func()));

        public static Try<Func<T2, R>> Map<T1, T2, R>(this Try<T1> @this, Func<T1, T2, R> map) =>
            @this.Map(map.Curry());

        public static Try<Func<T2, T3, R>> Map<T1, T2, T3, R>(this Try<T1> @this, Func<T1, T2, T3, R> map) =>
            @this.Map(map.CurryFirst());

        public static Try<Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R>(this Try<T1> @this, Func<T1, T2, T3, T4, R> map) =>
            @this.Map(map.CurryFirst());

        public static Try<Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R>(this Try<T1> @this, Func<T1, T2, T3, T4, T5, R> map) =>
            @this.Map(map.CurryFirst());

        // Monad

        public static Try<R> Bind<T, R>(this Try<T> @this, Func<T, Try<R>> bind) => bind(@this.func());

        // Apply

        public static Try<R> Apply<T, R>(this Try<Func<T, R>> @this, Try<T> arg) =>
            new(() => @this.func()(arg.func()));

        public static Try<Func<T2, R>> Apply<T1, T2, R>(this Try<Func<T1, T2, R>> @this, Try<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Try<Func<T2, T3, R>> Apply<T1, T2, T3, R>(this Try<Func<T1, T2, T3, R>> @this, Try<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Try<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>(this Try<Func<T1, T2, T3, T4, R>> @this, Try<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Try<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>(this Try<Func<T1, T2, T3, T4, T5, R>> @this, Try<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static Try<Unit> ForEach<T>(this Try<T> @this, Action<T> action) => @this.Map(action.ToFunc());

        // LINQ

        public static Try<R> Select<T, R>(this Try<T> @this, Func<T, R> map) => @this.Map(map);

        public static Try<R> SelectMany<T, R>(this Try<T> @this, Func<T, Try<R>> bind) => @this.Bind(bind);

        public static Try<S> SelectMany<T, R, S>(this Try<T> @this, Func<T, Try<R>> bind, Func<T, R, S> project) =>
            @this.Bind(t => bind(t).Map(r => project(t, r)));
    }
}
