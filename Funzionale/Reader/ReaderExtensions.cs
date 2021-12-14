namespace Funzionale
{
    public static class ReaderExtensions
    {
        // Functor

        public static Reader<Env, R> Map<Env, T, R>(this Reader<Env, T> @this, Func<T, R> map) where Env : struct =>
            new(env => map(@this.func(env)));

        // Monad

        public static Reader<Env, R> Bind<Env, T, R>(this Reader<Env, T> @this, Func<T, Reader<Env, R>> bind)
            where Env : struct =>
                new(env => bind(@this.func(env)).func(env));

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
