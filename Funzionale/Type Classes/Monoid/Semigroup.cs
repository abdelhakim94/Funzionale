namespace Funzionale
{
    public static partial class Prelude
    {
        public static T concat<SEMI, T>(T x, T y) where SEMI : struct, Semigroup<T> => default(SEMI).Concat(x, y);

        public static Either<L, R> concat<SEMI, L, R>(Either<L, R> ls, Either<L, R> rs) where SEMI : struct, Semigroup<R> =>
            from x in ls
            from y in rs
            select default(SEMI).Concat(x, y);

        public static Option<T> concat<SEMI, T>(Option<T> a, Option<T> b) where SEMI : struct, Semigroup<T> =>
            from x in a
            from y in b
            select default(SEMI).Concat(x, y);

        public static Validation<T> concat<SEMI, T>(Validation<T> a, Validation<T> b) where SEMI : struct, Semigroup<T> =>
            from x in a
            from y in b
            select default(SEMI).Concat(x, y);

        public static Exceptional<T> concat<SEMI, T>(Exceptional<T> a, Exceptional<T> b) where SEMI : struct, Semigroup<T> =>
            from x in a
            from y in b
            select default(SEMI).Concat(x, y);

        public static Reader<Env, T> concat<SEMI, Env, T>(Reader<Env, T> a, Reader<Env, T> b)
            where SEMI : struct, Semigroup<T>
            where Env : struct =>
                from x in a
                from y in b
                select default(SEMI).Concat(x, y);
    }

    public interface Semigroup<T>
    {
        T Concat(T x, T y);
    }
}
