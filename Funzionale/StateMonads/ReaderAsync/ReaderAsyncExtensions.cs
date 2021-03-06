namespace Funzionale
{
    public static class ReaderAsyncAsyncExtensions
    {
        // Functor

        public static ReaderAsync<Env, R> Map<Env, T, R>(this ReaderAsync<Env, T> @this, Func<T, R> map) where Env : struct =>
            new(env => @this.func(env).Map(map));

        public static ReaderAsync<Env, Func<T2, R>> Map<Env, T1, T2, R>(this ReaderAsync<Env, T1> @this, Func<T1, T2, R> f)
            where Env : struct =>
                @this.Map(f.Curry());

        public static ReaderAsync<Env, Func<T2, T3, R>> Map<Env, T1, T2, T3, R>(this ReaderAsync<Env, T1> @this, Func<T1, T2, T3, R> f)
            where Env : struct =>
                @this.Map(@f.CurryFirst());

        public static ReaderAsync<Env, Func<T2, T3, T4, R>> Map<Env, T1, T2, T3, T4, R>(this ReaderAsync<Env, T1> @this, Func<T1, T2, T3, T4, R> f)
            where Env : struct =>
                @this.Map(@f.CurryFirst());

        public static ReaderAsync<Env, Func<T2, T3, T4, T5, R>> Map<Env, T1, T2, T3, T4, T5, R>(this ReaderAsync<Env, T1> @this, Func<T1, T2, T3, T4, T5, R> f)
            where Env : struct =>
                @this.Map(@f.CurryFirst());

        // Monad

        public static ReaderAsync<Env, R> FlatMap<Env, T, R>(this ReaderAsync<Env, T> @this, Func<T, ReaderAsync<Env, R>> map)
            where Env : struct =>
                new(async env => await map(await @this.func(env).ConfigureAwait(false)).func(env).ConfigureAwait(false));

        // Applicative

        public static ReaderAsync<Env, R> Apply<Env, T, R>(this ReaderAsync<Env, Func<T, R>> @this, ReaderAsync<Env, T> arg)
            where Env : struct =>
                new(async env => (await @this.func(env).ConfigureAwait(false))(await arg.func(env).ConfigureAwait(false)));

        public static ReaderAsync<Env, Func<T2, R>> Apply<Env, T1, T2, R>(this ReaderAsync<Env, Func<T1, T2, R>> @this, ReaderAsync<Env, T1> arg)
            where Env : struct =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static ReaderAsync<Env, Func<T2, T3, R>> Apply<Env, T1, T2, T3, R>(this ReaderAsync<Env, Func<T1, T2, T3, R>> @this, ReaderAsync<Env, T1> arg)
            where Env : struct =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static ReaderAsync<Env, Func<T2, T3, T4, R>> Apply<Env, T1, T2, T3, T4, R>(this ReaderAsync<Env, Func<T1, T2, T3, T4, R>> @this, ReaderAsync<Env, T1> arg)
            where Env : struct =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static ReaderAsync<Env, Func<T2, T3, T4, T5, R>> Apply<Env, T1, T2, T3, T4, T5, R>(this ReaderAsync<Env, Func<T1, T2, T3, T4, T5, R>> @this, ReaderAsync<Env, T1> arg)
            where Env : struct =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static ReaderAsync<Env, Unit> ForEach<Env, T>(this ReaderAsync<Env, T> @this, Action<T> action)
            where Env : struct =>
                @this.Map(action.ToFunc());

        // TODO: Traversable

        // LINQ

        public static ReaderAsync<Env, R> Select<Env, T, R>(this ReaderAsync<Env, T> @this, Func<T, R> map) where Env : struct =>
            @this.Map(map);

        public static ReaderAsync<Env, R> SelectMany<Env, T, R>(this ReaderAsync<Env, T> @this, Func<T, ReaderAsync<Env, R>> map)
            where Env : struct =>
                @this.FlatMap(map);

        public static ReaderAsync<Env, S> SelectMany<Env, T, R, S>(this ReaderAsync<Env, T> @this, Func<T, ReaderAsync<Env, R>> map, Func<T, R, S> project)
            where Env : struct =>
                @this.FlatMap(t => map(t).Map(r => project(t, r)));
    }
}
