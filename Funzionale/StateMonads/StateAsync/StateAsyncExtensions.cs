namespace Funzionale
{
    public static partial class StateAsyncExtensions
    {
        // Functor

        public static StateAsync<S, R> Map<T, R, S>(this StateAsync<S, T> @this, Func<T, R> map) =>
            new(state => @this.func(state).Map(x => (map(x.value), x.state)));

        public static StateAsync<S, Func<T2, R>> Map<T1, T2, R, S>(this StateAsync<S, T1> @this, Func<T1, T2, R> map) =>
            @this.Map(map.Curry());

        public static StateAsync<S, Func<T2, T3, R>> Map<T1, T2, T3, R, S>(this StateAsync<S, T1> @this, Func<T1, T2, T3, R> map) =>
            @this.Map(map.CurryFirst());

        public static StateAsync<S, Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R, S>(this StateAsync<S, T1> @this, Func<T1, T2, T3, T4, R> map) =>
            @this.Map(map.CurryFirst());

        public static StateAsync<S, Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R, S>(this StateAsync<S, T1> @this, Func<T1, T2, T3, T4, T5, R> map) =>
            @this.Map(map.CurryFirst());

        // Monad

        public static StateAsync<S, R> Bind<T, R, S>(this StateAsync<S, T> @this, Func<T, StateAsync<S, R>> bind) =>
            new(state => @this.func(state).Map(x => bind(x.value).func(x.state)).Unwrap());

        // Applicative

        public static StateAsync<S, R> Apply<T, R, S>(this StateAsync<S, Func<T, R>> @this, StateAsync<S, T> arg) =>
            new(state => @this.func(state)
                .Map(x => arg.func(x.state)
                    .Map(y => (x.value(y.value), y.state)))
            .Unwrap());

        public static StateAsync<S, Func<T2, R>> Apply<T1, T2, R, S>(this StateAsync<S, Func<T1, T2, R>> @this, StateAsync<S, T1> arg) =>
            @this.Map(FuncExtensions.Curry).Apply(arg);

        public static StateAsync<S, Func<T2, T3, R>> Apply<T1, T2, T3, R, S>(this StateAsync<S, Func<T1, T2, T3, R>> @this, StateAsync<S, T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static StateAsync<S, Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R, S>(this StateAsync<S, Func<T1, T2, T3, T4, R>> @this, StateAsync<S, T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static StateAsync<S, Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R, S>(this StateAsync<S, Func<T1, T2, T3, T4, T5, R>> @this, StateAsync<S, T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static StateAsync<S, Unit> ForEach<T, S>(this StateAsync<S, T> @this, Action<T> action) => @this.Map(action.ToFunc());

        // LINQ

        public static StateAsync<S, R> Select<T, R, S>(this StateAsync<S, T> @this, Func<T, R> map) => @this.Map(map);

        public static StateAsync<S, R> SelectMany<T, R, S>(this StateAsync<S, T> @this, Func<T, StateAsync<S, R>> bind) => @this.Bind(bind);

        public static StateAsync<S, V> SelectMany<T, R, V, S>(this StateAsync<S, T> @this, Func<T, StateAsync<S, R>> bind, Func<T, R, V> project) =>
                @this.Bind(t => bind(t).Map(r => project(t, r)));
    }
}
