namespace Funzionale
{
    using static Prelude;

    public static class RwsAsyncAsyncExtensions
    {
        // Functor

        public static RwsAsync<MonoidW, Env, W, S, R> Map<MonoidW, Env, W, S, T, R>(
            this RwsAsync<MonoidW, Env, W, S, T> @this, Func<T, R> map) where MonoidW : struct, Monoid<W> =>
                new((env, state) => @this.func(env, state).Map(t => (map(t.value), t.output, t.state)));

        public static RwsAsync<MonoidW, Env, W, S, Func<T2, R>> Map<MonoidW, Env, W, S, T1, T2, R>(
            this RwsAsync<MonoidW, Env, W, S, T1> @this, Func<T1, T2, R> map) where MonoidW : struct, Monoid<W> =>
                @this.Map(map.Curry());

        public static RwsAsync<MonoidW, Env, W, S, Func<T2, T3, R>> Map<MonoidW, Env, W, S, T1, T2, T3, R>(
            this RwsAsync<MonoidW, Env, W, S, T1> @this, Func<T1, T2, T3, R> map) where MonoidW : struct, Monoid<W> =>
                @this.Map(map.CurryFirst());

        public static RwsAsync<MonoidW, Env, W, S, Func<T2, T3, T4, R>> Map<MonoidW, Env, W, S, T1, T2, T3, T4, R>(
            this RwsAsync<MonoidW, Env, W, S, T1> @this, Func<T1, T2, T3, T4, R> map) where MonoidW : struct, Monoid<W> =>
                @this.Map(map.CurryFirst());

        public static RwsAsync<MonoidW, Env, W, S, Func<T2, T3, T4, T5, R>> Map<MonoidW, Env, W, S, T1, T2, T3, T4, T5, R>(
            this RwsAsync<MonoidW, Env, W, S, T1> @this, Func<T1, T2, T3, T4, T5, R> map) where MonoidW : struct, Monoid<W> =>
                @this.Map(map.CurryFirst());

        // Monad

        public static RwsAsync<MonoidW, Env, W, S, R> FlatMap<MonoidW, Env, W, S, T, R>(
            this RwsAsync<MonoidW, Env, W, S, T> @this, Func<T, RwsAsync<MonoidW, Env, W, S, R>> bind) where MonoidW : struct, Monoid<W> =>
                new((env, state) => @this.func(env, state)
                    .Map(x => bind(x.value).func(env, x.state)
                        .Map(y => (y.value, concat<MonoidW, W>(x.output, y.output), y.state)))
                .Unwrap());

        // Applicative

        public static RwsAsync<MonoidW, Env, W, S, R> Apply<MonoidW, Env, W, S, T, R>(
            this RwsAsync<MonoidW, Env, W, S, Func<T, R>> @this, RwsAsync<MonoidW, Env, W, S, T> arg) where MonoidW : struct, Monoid<W> =>
                new((env, state) => @this.func(env, state)
                    .Map(x => arg.func(env, x.state)
                        .Map(y => (x.value(y.value), concat<MonoidW, W>(x.output, y.output), y.state)))
                .Unwrap());

        public static RwsAsync<MonoidW, Env, W, S, Func<T2, R>> Apply<MonoidW, Env, W, S, T1, T2, R>(
            this RwsAsync<MonoidW, Env, W, S, Func<T1, T2, R>> @this, RwsAsync<MonoidW, Env, W, S, T1> arg)
                where MonoidW : struct, Monoid<W> =>
                    @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static RwsAsync<MonoidW, Env, W, S, Func<T2, T3, R>> Apply<MonoidW, Env, W, S, T1, T2, T3, R>(
            this RwsAsync<MonoidW, Env, W, S, Func<T1, T2, T3, R>> @this, RwsAsync<MonoidW, Env, W, S, T1> arg) where MonoidW : struct, Monoid<W> =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static RwsAsync<MonoidW, Env, W, S, Func<T2, T3, T4, R>> Apply<MonoidW, Env, W, S, T1, T2, T3, T4, R>(
            this RwsAsync<MonoidW, Env, W, S, Func<T1, T2, T3, T4, R>> @this, RwsAsync<MonoidW, Env, W, S, T1> arg) where MonoidW : struct, Monoid<W> =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static RwsAsync<MonoidW, Env, W, S, Func<T2, T3, T4, T5, R>> Apply<MonoidW, Env, W, S, T1, T2, T3, T4, T5, R>(
            this RwsAsync<MonoidW, Env, W, S, Func<T1, T2, T3, T4, T5, R>> @this, RwsAsync<MonoidW, Env, W, S, T1> arg) where MonoidW : struct, Monoid<W> =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static RwsAsync<MonoidW, Env, W, S, Unit> ForEach<MonoidW, Env, W, S, T>(
            this RwsAsync<MonoidW, Env, W, S, T> @this, Action<T> action) where MonoidW : struct, Monoid<W> =>
                @this.Map(action.ToFunc());

        // LINQ

        public static RwsAsync<MonoidW, Env, W, S, R> Select<MonoidW, Env, W, S, T, R>(
            this RwsAsync<MonoidW, Env, W, S, T> @this, Func<T, R> map) where MonoidW : struct, Monoid<W> => @this.Map(map);

        public static RwsAsync<MonoidW, Env, W, S, R> SelectMany<MonoidW, Env, W, S, T, R>(
            this RwsAsync<MonoidW, Env, W, S, T> @this, Func<T, RwsAsync<MonoidW, Env, W, S, R>> bind) where MonoidW : struct, Monoid<W> =>
                @this.FlatMap(bind);

        public static RwsAsync<MonoidW, Env, W, S, V> SelectMany<MonoidW, Env, W, S, T, R, V>(
            this RwsAsync<MonoidW, Env, W, S, T> @this,
            Func<T, RwsAsync<MonoidW, Env, W, S, R>> bind,
            Func<T, R, V> project) where MonoidW : struct, Monoid<W> =>
                @this.FlatMap(t => bind(t).Map(r => project(t, r)));
    }
}
