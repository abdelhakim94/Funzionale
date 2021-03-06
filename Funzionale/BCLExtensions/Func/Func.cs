using System.Linq.Expressions;

namespace Funzionale
{
    public static partial class Prelude
    {
        public static Func<R> fun<R>(Func<R> f) => f;
        public static Func<T, R> fun<T, R>(Func<T, R> f) => f;
        public static Func<T1, T2, R> fun<T1, T2, R>(Func<T1, T2, R> f) => f;
        public static Func<T1, T2, T3, R> fun<T1, T2, T3, R>(Func<T1, T2, T3, R> f) => f;
        public static Func<T1, T2, T3, T4, R> fun<T1, T2, T3, T4, R>(Func<T1, T2, T3, T4, R> f) => f;
        public static Func<T1, T2, T3, T4, T5, R> fun<T1, T2, T3, T4, T5, R>(Func<T1, T2, T3, T4, T5, R> f) => f;

        public static Expression<Func<R>> expr<R>(Func<R> f) => () => f();
        public static Expression<Func<T, R>> expr<T, R>(Func<T, R> f) => t => f(t);
        public static Expression<Func<T1, T2, R>> expr<T1, T2, R>(Func<T1, T2, R> f) => (t1, t2) => f(t1, t2);
        public static Expression<Func<T1, T2, T3, R>> expr<T1, T2, T3, R>(Func<T1, T2, T3, R> f) =>
            (t1, t2, t3) => f(t1, t2, t3);

        public static Expression<Func<T1, T2, T3, T4, R>> expr<T1, T2, T3, T4, R>(Func<T1, T2, T3, T4, R> f) =>
            (t1, t2, t3, t4) => f(t1, t2, t3, t4);

        public static Expression<Func<T1, T2, T3, T4, T5, R>> expr<T1, T2, T3, T4, T5, R>(Func<T1, T2, T3, T4, T5, R> f) =>
            (t1, t2, t3, t4, t5) => f(t1, t2, t3, t4, t5);
    }

    public static class FuncExtensions
    {
        // Curry

        public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> @this) =>
            t1 => t2 => @this(t1, t2);

        public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(this Func<T1, T2, T3, R> @this) =>
            t1 => t2 => t3 => @this(t1, t2, t3);

        public static Func<T1, Func<T2, Func<T3, Func<T4, R>>>> Curry<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> @this) =>
            t1 => t2 => t3 => t4 => @this(t1, t2, t3, t4);

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, R>>>>> Curry<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> @this) =>
            t1 => t2 => t3 => t4 => t5 => @this(t1, t2, t3, t4, t5);

        public static Func<T1, Func<T2, R>> CurryFirst<T1, T2, R>(this Func<T1, T2, R> @this) =>
            @this.Curry();

        public static Func<T1, Func<T2, T3, R>> CurryFirst<T1, T2, T3, R>(this Func<T1, T2, T3, R> @this) =>
            t1 => (t2, t3) => @this(t1, t2, t3);

        public static Func<T1, Func<T2, T3, T4, R>> CurryFirst<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> @this) =>
            t1 => (t2, t3, t4) => @this(t1, t2, t3, t4);

        public static Func<T1, Func<T2, T3, T4, T5, R>> CurryFirst<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> @this) =>
            t1 => (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);

        // Apply
        public static R Apply<T, R>(this Func<T, R> @this, T t) => @this(t);

        public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> @this, T1 t1) =>
            t2 => @this(t1, t2);

        public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> @this, T1 t1) =>
            (t2, t3) => @this(t1, t2, t3);

        public static Func<T2, T3, T4, R> Apply<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> @this, T1 t1) =>
            (t2, t3, t4) => @this(t1, t2, t3, t4);

        public static Func<T2, T3, T4, T5, R> Apply<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> @this, T1 t1) =>
            (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);

        // Make Func a functor over the result

        public static Func<R> Map<T, R>(this Func<T> @this, Func<T, R> map) => () => map(@this());

        public static Func<T1, R> Map<T1, T2, R>(this Func<T1, T2> @this, Func<T2, R> map) => t1 => map(@this(t1));

        public static Func<T1, T2, R> Map<T1, T2, T3, R>(this Func<T1, T2, T3> @this, Func<T3, R> map) =>
            (t1, t2) => map(@this(t1, t2));

        public static Func<T1, T2, T3, R> Map<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4> @this, Func<T4, R> map) =>
            (t1, t2, t3) => map(@this(t1, t2, t3));

        public static Func<T1, T2, T3, T4, R> Map<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5> @this, Func<T5, R> map) =>
            (t1, t2, t3, t4) => map(@this(t1, t2, t3, t4));

        public static Func<T1, T2, T3, T4, T5, R> Map<T1, T2, T3, T4, T5, T6, R>(this Func<T1, T2, T3, T4, T5, T6> @this, Func<T6, R> map) =>
            (t1, t2, t3, t4, t5) => map(@this(t1, t2, t3, t4, t5));

        // Make Func a monad over the result

        public static Func<R> FlatMap<T, R>(this Func<T> @this, Func<T, Func<R>> map) => () => map(@this())();

        public static Func<T1, R> FlatMap<T1, T2, R>(this Func<T1, T2> @this, Func<T2, Func<R>> map) => t1 => map(@this(t1))();

        public static Func<T1, T2, R> FlatMap<T1, T2, T3, R>(this Func<T1, T2, T3> @this, Func<T3, Func<R>> map) =>
            (t1, t2) => map(@this(t1, t2))();

        public static Func<T1, T2, T3, R> FlatMap<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4> @this, Func<T4, Func<R>> map) =>
            (t1, t2, t3) => map(@this(t1, t2, t3))();

        public static Func<T1, T2, T3, T4, R> FlatMap<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5> @this, Func<T5, Func<R>> map) =>
            (t1, t2, t3, t4) => map(@this(t1, t2, t3, t4))();

        public static Func<T1, T2, T3, T4, T5, R> FlatMap<T1, T2, T3, T4, T5, T6, R>(this Func<T1, T2, T3, T4, T5, T6> @this, Func<T6, Func<R>> map) =>
            (t1, t2, t3, t4, t5) => map(@this(t1, t2, t3, t4, t5))();

        // LINQ

        public static Func<R> Select<T, R>(this Func<T> @this, Func<T, R> map) =>
            @this.Map(map);

        public static Func<T1, R> Select<T1, T2, R>(this Func<T1, T2> @this, Func<T2, R> map) =>
            @this.Map(map);

        public static Func<T1, T2, R> Select<T1, T2, T3, R>(this Func<T1, T2, T3> @this, Func<T3, R> map) =>
            @this.Map(map);

        public static Func<T1, T2, T3, R> Select<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4> @this, Func<T4, R> map) =>
            @this.Map(map);

        public static Func<T1, T2, T3, T4, R> Select<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5> @this, Func<T5, R> map) =>
            @this.Map(map);

        public static Func<T1, T2, T3, T4, T5, R> Select<T1, T2, T3, T4, T5, T6, R>(this Func<T1, T2, T3, T4, T5, T6> @this, Func<T6, R> map) =>
            @this.Map(map);

        public static Func<S> SelectMany<T, R, S>(this Func<T> @this, Func<T, Func<R>> map, Func<T, R, S> project) =>
            @this.FlatMap(t => map(t).Map(r => project(t, r)));

        public static Func<T1, S> SelectMany<T1, T2, R, S>(this Func<T1, T2> @this, Func<T2, Func<R>> map, Func<T2, R, S> project) =>
            @this.FlatMap(t => map(t).Map(r => project(t, r)));

        public static Func<T1, T2, S> SelectMany<T1, T2, T3, R, S>(this Func<T1, T2, T3> @this, Func<T3, Func<R>> map, Func<T3, R, S> project) =>
            @this.FlatMap(t => map(t).Map(r => project(t, r)));

        public static Func<T1, T2, T3, S> SelectMany<T1, T2, T3, T4, R, S>(this Func<T1, T2, T3, T4> @this, Func<T4, Func<R>> map, Func<T4, R, S> project) =>
            @this.FlatMap(t => map(t).Map(r => project(t, r)));

        public static Func<T1, T2, T3, T4, S> SelectMany<T1, T2, T3, T4, T5, R, S>(this Func<T1, T2, T3, T4, T5> @this, Func<T5, Func<R>> map, Func<T5, R, S> project) =>
            @this.FlatMap(t => map(t).Map(r => project(t, r)));

        public static Func<T1, T2, T3, T4, T5, S> SelectMany<T1, T2, T3, T4, T5, T6, R, S>(this Func<T1, T2, T3, T4, T5, T6> @this, Func<T6, Func<R>> map, Func<T6, R, S> project) =>
            @this.FlatMap(t => map(t).Map(r => project(t, r)));
    }
}
