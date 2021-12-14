namespace Funzionale
{
    public static class ReaderExtensions
    {
        // Functor

        public static Reader<Env, R> Map<Env, T, R>(this Reader<Env, T> @this, Func<T, R> map) where Env : struct =>
            new(env => map(@this.func(env)));

        public static Reader<Env, Func<T2, R>> Map<Env, T1, T2, R>(this Reader<Env, T1> @this, Func<T1, T2, R> f)
            where Env : struct =>
                @this.Map(f.Curry());

        public static Reader<Env, Func<T2, T3, R>> Map<Env, T1, T2, T3, R>(this Reader<Env, T1> @this, Func<T1, T2, T3, R> f)
            where Env : struct =>
                @this.Map(@f.CurryFirst());

        public static Reader<Env, Func<T2, T3, T4, R>> Map<Env, T1, T2, T3, T4, R>(this Reader<Env, T1> @this, Func<T1, T2, T3, T4, R> f)
            where Env : struct =>
                @this.Map(@f.CurryFirst());

        public static Reader<Env, Func<T2, T3, T4, T5, R>> Map<Env, T1, T2, T3, T4, T5, R>(this Reader<Env, T1> @this, Func<T1, T2, T3, T4, T5, R> f)
            where Env : struct =>
                @this.Map(@f.CurryFirst());

        // Monad

        public static Reader<Env, R> Bind<Env, T, R>(this Reader<Env, T> @this, Func<T, Reader<Env, R>> bind)
            where Env : struct =>
                new(env => bind(@this.func(env)).func(env));

        // Applicative

        public static Reader<Env, R> Apply<Env, T, R>(this Reader<Env, Func<T, R>> @this, Reader<Env, T> arg)
            where Env : struct =>
                new(env => @this.func(env)(arg.func(env)));

        public static Reader<Env, Func<T2, R>> Apply<Env, T1, T2, R>(this Reader<Env, Func<T1, T2, R>> @this, Reader<Env, T1> arg)
            where Env : struct =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Reader<Env, Func<T2, T3, R>> Apply<Env, T1, T2, T3, R>(this Reader<Env, Func<T1, T2, T3, R>> @this, Reader<Env, T1> arg)
            where Env : struct =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Reader<Env, Func<T2, T3, T4, R>> Apply<Env, T1, T2, T3, T4, R>(this Reader<Env, Func<T1, T2, T3, T4, R>> @this, Reader<Env, T1> arg)
            where Env : struct =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Reader<Env, Func<T2, T3, T4, T5, R>> Apply<Env, T1, T2, T3, T4, T5, R>(this Reader<Env, Func<T1, T2, T3, T4, T5, R>> @this, Reader<Env, T1> arg)
            where Env : struct =>
                @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static Reader<Env, Unit> ForEach<Env, T>(this Reader<Env, T> @this, Action<T> action)
            where Env : struct =>
                @this.Map(action.ToFunc());

        // TODO: Traversable

        // LINQ

        public static Reader<Env, R> Select<Env, T, R>(this Reader<Env, T> @this, Func<T, R> map) where Env : struct =>
            @this.Map(map);

        public static Reader<Env, R> SelectMany<Env, T, R>(this Reader<Env, T> @this, Func<T, Reader<Env, R>> bind)
            where Env : struct =>
                @this.Bind(bind);

        public static Reader<Env, S> SelectMany<Env, T, R, S>(this Reader<Env, T> @this, Func<T, Reader<Env, R>> bind, Func<T, R, S> project)
            where Env : struct =>
                @this.Bind(t => bind(t).Map(r => project(t, r)));
    }
}
