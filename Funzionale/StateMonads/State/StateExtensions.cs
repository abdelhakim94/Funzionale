namespace Funzionale
{
    public static partial class StateExtensions
    {
        // Functor

        public static State<S, R> Map<T, R, S>(this State<S, T> @this, Func<T, R> map) =>
            new(state => @this.func(state).Transform(x => (map(x.value), x.state)));

        public static State<S, Func<T2, R>> Map<T1, T2, R, S>(this State<S, T1> @this, Func<T1, T2, R> map) =>
            @this.Map(map.Curry());

        public static State<S, Func<T2, T3, R>> Map<T1, T2, T3, R, S>(this State<S, T1> @this, Func<T1, T2, T3, R> map) =>
            @this.Map(map.CurryFirst());

        public static State<S, Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R, S>(this State<S, T1> @this, Func<T1, T2, T3, T4, R> map) =>
            @this.Map(map.CurryFirst());

        public static State<S, Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R, S>(this State<S, T1> @this, Func<T1, T2, T3, T4, T5, R> map) =>
            @this.Map(map.CurryFirst());

        // Monad

        public static State<S, R> FlatMap<T, R, S>(this State<S, T> @this, Func<T, State<S, R>> map) =>
            new(state => @this.func(state).Transform(x => map(x.value).func(x.state)));

        // Applicative

        public static State<S, R> Apply<T, R, S>(this State<S, Func<T, R>> @this, State<S, T> arg) =>
            new(state => @this.func(state)
                .Transform(x => arg.func(x.state)
                    .Transform(y => (x.value(y.value), y.state))));

        public static State<S, Func<T2, R>> Apply<T1, T2, R, S>(this State<S, Func<T1, T2, R>> @this, State<S, T1> arg) =>
            @this.Map(FuncExtensions.Curry).Apply(arg);

        public static State<S, Func<T2, T3, R>> Apply<T1, T2, T3, R, S>(this State<S, Func<T1, T2, T3, R>> @this, State<S, T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static State<S, Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R, S>(this State<S, Func<T1, T2, T3, T4, R>> @this, State<S, T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static State<S, Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R, S>(this State<S, Func<T1, T2, T3, T4, T5, R>> @this, State<S, T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static State<S, Unit> ForEach<T, S>(this State<S, T> @this, Action<T> action) => @this.Map(action.ToFunc());

        // LINQ

        public static State<S, R> Select<T, R, S>(this State<S, T> @this, Func<T, R> map) => @this.Map(map);

        public static State<S, R> SelectMany<T, R, S>(this State<S, T> @this, Func<T, State<S, R>> map) => @this.FlatMap(map);

        public static State<S, V> SelectMany<T, R, V, S>(this State<S, T> @this, Func<T, State<S, R>> map, Func<T, R, V> project) =>
                @this.FlatMap(t => map(t).Map(r => project(t, r)));
    }
}
