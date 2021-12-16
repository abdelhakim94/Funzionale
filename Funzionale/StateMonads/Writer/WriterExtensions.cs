namespace Funzionale
{
    using static Prelude;
    public static class WriterExtensions
    {
        // Functor

        public static Writer<MonoidW, W, R> Map<MonoidW, W, T, R>(this Writer<MonoidW, W, T> @this, Func<T, R> map)
            where MonoidW : struct, Monoid<W> =>
                fun(() => @this.func().Transform(t => (map(t.value), t.output)));

        public static Writer<MonoidW, W, Func<T2, R>> Map<MonoidW, W, T1, T2, R>(
            this Writer<MonoidW, W, T1> @this, Func<T1, T2, R> f) where MonoidW : struct, Monoid<W> =>
                @this.Map(f.Curry());

        public static Writer<MonoidW, W, Func<T2, T3, R>> Map<MonoidW, W, T1, T2, T3, R>(
            this Writer<MonoidW, W, T1> @this, Func<T1, T2, T3, R> f) where MonoidW : struct, Monoid<W> =>
                @this.Map(f.CurryFirst());

        public static Writer<MonoidW, W, Func<T2, T3, T4, R>> Map<MonoidW, W, T1, T2, T3, T4, R>(
            this Writer<MonoidW, W, T1> @this, Func<T1, T2, T3, T4, R> f) where MonoidW : struct, Monoid<W> =>
                @this.Map(f.CurryFirst());

        public static Writer<MonoidW, W, Func<T2, T3, T4, T5, R>> Map<MonoidW, W, T1, T2, T3, T4, T5, R>(
            this Writer<MonoidW, W, T1> @this, Func<T1, T2, T3, T4, T5, R> f) where MonoidW : struct, Monoid<W> =>
                @this.Map(f.CurryFirst());

        // Monad

        public static Writer<MonoidW, W, R> Bind<MonoidW, W, T, R>(
            this Writer<MonoidW, W, T> @this, Func<T, Writer<MonoidW, W, R>> bind)
                where MonoidW : struct, Monoid<W> =>
                    fun(() => @this.func()
                        .Transform(x => bind(x.value).func()
                            .Transform(y => (y.value, concat<MonoidW, W>(x.output, y.output)))));

        // Applicative

        public static Writer<MonoidW, W, R> Apply<MonoidW, W, T, R>(
            this Writer<MonoidW, W, Func<T, R>> @this, Writer<MonoidW, W, T> arg) where MonoidW : struct, Monoid<W> =>
                fun(() => @this.func()
                    .Transform(x => arg.func()
                        .Transform(y => (x.value(y.value), concat<MonoidW, W>(x.output, y.output)))));

        public static Writer<MonoidW, W, Func<T2, R>> Apply<MonoidW, W, T1, T2, R>(
            this Writer<MonoidW, W, Func<T1, T2, R>> @this, Writer<MonoidW, W, T1> arg) where MonoidW : struct, Monoid<W> =>
                @this.Map(FuncExtensions.Curry).Apply(arg);

        public static Writer<MonoidW, W, Func<T2, T3, R>> Apply<MonoidW, W, T1, T2, T3, R>(
            this Writer<MonoidW, W, Func<T1, T2, T3, R>> @this, Writer<MonoidW, W, T1> arg) where MonoidW : struct, Monoid<W> =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Writer<MonoidW, W, Func<T2, T3, T4, R>> Apply<MonoidW, W, T1, T2, T3, T4, R>(
            this Writer<MonoidW, W, Func<T1, T2, T3, T4, R>> @this, Writer<MonoidW, W, T1> arg)
                where MonoidW : struct, Monoid<W> =>
                    @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Writer<MonoidW, W, Func<T2, T3, T4, T5, R>> Apply<MonoidW, W, T1, T2, T3, T4, T5, R>(
            this Writer<MonoidW, W, Func<T1, T2, T3, T4, T5, R>> @this, Writer<MonoidW, W, T1> arg)
                where MonoidW : struct, Monoid<W> =>
                    @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static Writer<MonoidW, W, Unit> ForEach<MonoidW, W, T>(this Writer<MonoidW, W, T> @this, Action<T> action)
            where MonoidW : struct, Monoid<W> => @this.Map(action.ToFunc());

        // LINQ
        public static Writer<MonoidW, W, R> Select<MonoidW, W, T, R>(this Writer<MonoidW, W, T> @this, Func<T, R> map)
            where MonoidW : struct, Monoid<W> => @this.Map(map);

        public static Writer<MonoidW, W, R> SelectMany<MonoidW, W, T, R>(
            this Writer<MonoidW, W, T> @this, Func<T, Writer<MonoidW, W, R>> bind) where MonoidW : struct, Monoid<W> =>
                @this.Bind(bind);

        public static Writer<MonoidW, W, S> SelectMany<MonoidW, W, T, R, S>(
            this Writer<MonoidW, W, T> @this, Func<T, Writer<MonoidW, W, R>> bind, Func<T, R, S> project)
                where MonoidW : struct, Monoid<W> => @this.Bind(t => bind(t).Map(r => project(t, r)));
    }
}
