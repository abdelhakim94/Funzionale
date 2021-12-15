namespace Funzionale
{
    public static partial class Prelude
    {
        public static T mempty<MONOID, T>() where MONOID : struct, Monoid<T> => default(MONOID).Empty();

        public static T mconcat<MONOID, T>(IEnumerable<T> ts) where MONOID : struct, Monoid<T> =>
            ts.Aggregate(mempty<MONOID, T>(), (x, y) => concat<MONOID, T>(x, y));

        public static T mconcat<MONOID, T>(params T[] ts) where MONOID : struct, Monoid<T> =>
            ts.Aggregate(mempty<MONOID, T>(), (x, y) => concat<MONOID, T>(x, y));
    }

    public interface Monoid<T> : Semigroup<T>
    {
        T Empty();
    }
}
