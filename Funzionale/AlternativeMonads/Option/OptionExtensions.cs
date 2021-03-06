using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public static class OptionExtensions
    {
        /// <summary>
        /// We could have let the null value returned by a function be lifted with some,
        /// and that would have thrown an exception anyway. But we want to give
        /// the caller a clear indication that the problem comes from the fact that the function
        /// he provided returns null and that he should use Option instead.
        /// </summary>
        [DoesNotReturn]
        private static T Guard<T>(string fName) => throw new ArgumentNullException($"'{fName}' returned null. Consider returning an Option instead.");

        // Functor

        public static Option<R> Map<T, R>(this Option.None _, Func<T, R> map) => none;
        public static Option<R> Map<T, R>(this Option<T> @this, Func<T, R> map) =>
            @this.Match(
                None: () => none,
                Some: t => some(map(t) ?? Guard<R>(nameof(map))));

        public static Option<Func<T2, R>> Map<T1, T2, R>(this Option<T1> @this, Func<T1, T2, R> f) =>
            @this.Map(f.CurryFirst());

        public static Option<Func<T2, T3, R>> Map<T1, T2, T3, R>(this Option<T1> @this, Func<T1, T2, T3, R> f) =>
            @this.Map(f.CurryFirst());

        public static Option<Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R>(this Option<T1> @this, Func<T1, T2, T3, T4, R> f) =>
            @this.Map(f.CurryFirst());

        public static Option<Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R>(this Option<T1> @this, Func<T1, T2, T3, T4, T5, R> f) =>
            @this.Map(f.CurryFirst());

        // Monad

        public static Option<R> FlatMap<T, R>(this Option<T> @this, Func<T, Option<R>> map) =>
            @this.Match(
                None: () => none,
                Some: v => map(v));

        // Applicative

        public static Option<R> Apply<T, R>(this Option<Func<T, R>> @this, Option<T> arg) =>
            @this.Match(
                None: () => none,
                Some: innerFunc => arg.Map(innerFunc));

        public static Option<R> Apply<T, R>(this Option<Func<T, Option<R>>> @this, Option<T> arg) =>
            @this.Match(
                None: () => none,
                Some: f => arg.FlatMap(f));

        public static Option<Func<T2, R>> Apply<T1, T2, R>(this Option<Func<T1, T2, R>> @this, Option<T1> arg) =>
            @this.Map(FuncExtensions.Curry).Apply(arg);

        public static Option<Func<T2, T3, R>> Apply<T1, T2, T3, R>(this Option<Func<T1, T2, T3, R>> @this, Option<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Option<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>(this Option<Func<T1, T2, T3, T4, T5, R>> @this, Option<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Option<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>(this Option<Func<T1, T2, T3, T4, R>> @this, Option<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action) => @this.Map(action.ToFunc());

        public static Unit Match<T>(this Option<T> @this, Action None, Action<T> Some) =>
            @this.Match(None.ToFunc(), Some.ToFunc());

        // TODO: Transormations of option to validation

        // TODO: Make option traversable with other common monads like Task, IEnumerable...etc.

        // LINQ

        public static Option<R> Select<T, R>(this Option<T> @this, Func<T, R> map) => @this.Map(map);

        public static Option<R> SelectMany<T, R>(this Option<T> @this, Func<T, Option<R>> map) => @this.FlatMap(map);

        public static Option<S> SelectMany<T, R, S>(this Option<T> @this, Func<T, Option<R>> map, Func<T, R, S> project) =>
            @this.FlatMap(x => map(x).Map(y => project(x, y)));

        public static Option<T> Where<T>(this Option<T> @this, Func<T, bool> predicate) =>
            @this.Match(
                None: () => none,
                Some: v => predicate(v) ? some(v!) : none);
    }
}
