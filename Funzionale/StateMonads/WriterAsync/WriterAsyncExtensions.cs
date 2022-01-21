namespace Funzionale
{
    using static Prelude;
    public static class WriterAsyncExtensions
    {
        // Functor

        public static WriterAsync<MonoidW, W, R> Map<MonoidW, W, T, R>(this WriterAsync<MonoidW, W, T> @this, Func<T, R> map)
            where MonoidW : struct, Monoid<W> =>
                new(() => @this.func().Map(t => (map(t.value), t.output)));

        public static WriterAsync<MonoidW, W, Func<T2, R>> Map<MonoidW, W, T1, T2, R>(
            this WriterAsync<MonoidW, W, T1> @this, Func<T1, T2, R> f) where MonoidW : struct, Monoid<W> =>
                @this.Map(f.Curry());

        public static WriterAsync<MonoidW, W, Func<T2, T3, R>> Map<MonoidW, W, T1, T2, T3, R>(
            this WriterAsync<MonoidW, W, T1> @this, Func<T1, T2, T3, R> f) where MonoidW : struct, Monoid<W> =>
                @this.Map(f.CurryFirst());

        public static WriterAsync<MonoidW, W, Func<T2, T3, T4, R>> Map<MonoidW, W, T1, T2, T3, T4, R>(
            this WriterAsync<MonoidW, W, T1> @this, Func<T1, T2, T3, T4, R> f) where MonoidW : struct, Monoid<W> =>
                @this.Map(f.CurryFirst());

        public static WriterAsync<MonoidW, W, Func<T2, T3, T4, T5, R>> Map<MonoidW, W, T1, T2, T3, T4, T5, R>(
            this WriterAsync<MonoidW, W, T1> @this, Func<T1, T2, T3, T4, T5, R> f) where MonoidW : struct, Monoid<W> =>
                @this.Map(f.CurryFirst());

        // Monad

        public static WriterAsync<MonoidW, W, R> FlatMap<MonoidW, W, T, R>(
            this WriterAsync<MonoidW, W, T> @this, Func<T, WriterAsync<MonoidW, W, R>> map)
                where MonoidW : struct, Monoid<W> =>
                    new(() => @this.func()
                        .Map(x => map(x.value).func()
                            .Map(y => (y.value, concat<MonoidW, W>(x.output, y.output))))
                    .Unwrap());

        // Applicative

        public static WriterAsync<MonoidW, W, R> Apply<MonoidW, W, T, R>(
            this WriterAsync<MonoidW, W, Func<T, R>> @this, WriterAsync<MonoidW, W, T> arg) where MonoidW : struct, Monoid<W> =>
                new(() => @this.func()
                    .Map(x => arg.func()
                        .Map(y => (x.value(y.value), concat<MonoidW, W>(x.output, y.output))))
                .Unwrap());

        public static WriterAsync<MonoidW, W, Func<T2, R>> Apply<MonoidW, W, T1, T2, R>(
            this WriterAsync<MonoidW, W, Func<T1, T2, R>> @this, WriterAsync<MonoidW, W, T1> arg) where MonoidW : struct, Monoid<W> =>
                @this.Map(FuncExtensions.Curry).Apply(arg);

        public static WriterAsync<MonoidW, W, Func<T2, T3, R>> Apply<MonoidW, W, T1, T2, T3, R>(
            this WriterAsync<MonoidW, W, Func<T1, T2, T3, R>> @this, WriterAsync<MonoidW, W, T1> arg) where MonoidW : struct, Monoid<W> =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static WriterAsync<MonoidW, W, Func<T2, T3, T4, R>> Apply<MonoidW, W, T1, T2, T3, T4, R>(
            this WriterAsync<MonoidW, W, Func<T1, T2, T3, T4, R>> @this, WriterAsync<MonoidW, W, T1> arg)
                where MonoidW : struct, Monoid<W> =>
                    @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static WriterAsync<MonoidW, W, Func<T2, T3, T4, T5, R>> Apply<MonoidW, W, T1, T2, T3, T4, T5, R>(
            this WriterAsync<MonoidW, W, Func<T1, T2, T3, T4, T5, R>> @this, WriterAsync<MonoidW, W, T1> arg)
                where MonoidW : struct, Monoid<W> =>
                    @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static WriterAsync<MonoidW, W, Unit> ForEach<MonoidW, W, T>(this WriterAsync<MonoidW, W, T> @this, Action<T> action)
            where MonoidW : struct, Monoid<W> => @this.Map(action.ToFunc());

        // LINQ
        public static WriterAsync<MonoidW, W, R> Select<MonoidW, W, T, R>(this WriterAsync<MonoidW, W, T> @this, Func<T, R> map)
            where MonoidW : struct, Monoid<W> => @this.Map(map);

        public static WriterAsync<MonoidW, W, R> SelectMany<MonoidW, W, T, R>(
            this WriterAsync<MonoidW, W, T> @this, Func<T, WriterAsync<MonoidW, W, R>> map) where MonoidW : struct, Monoid<W> =>
                @this.FlatMap(map);

        public static WriterAsync<MonoidW, W, S> SelectMany<MonoidW, W, T, R, S>(
            this WriterAsync<MonoidW, W, T> @this, Func<T, WriterAsync<MonoidW, W, R>> map, Func<T, R, S> project)
                where MonoidW : struct, Monoid<W> => @this.FlatMap(t => map(t).Map(r => project(t, r)));
    }
}
