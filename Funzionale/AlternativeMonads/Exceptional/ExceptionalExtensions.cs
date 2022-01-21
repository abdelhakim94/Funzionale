using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public static class ExceptionalExtensions
    {
        [DoesNotReturn]
        private static T Guard<T>(string fName) => throw new ArgumentNullException(fName, $"{fName} returned a null. Consider returning an Option.");

        // Functor

        public static Exceptional<R> Map<T, R>(this Exceptional<T> @this, Func<T, R> map) =>
            @this.Match(
                Exception: ex => failure<R>(ex),
                Success: t => success(map(t) ?? Guard<R>(nameof(map))));

        public static Exceptional<Func<T2, R>> Map<T1, T2, R>(this Exceptional<T1> @this, Func<T1, T2, R> f) =>
            @this.Map(f.Curry());

        public static Exceptional<Func<T2, T3, R>> Map<T1, T2, T3, R>(this Exceptional<T1> @this, Func<T1, T2, T3, R> f) =>
            @this.Map(f.CurryFirst());

        public static Exceptional<Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R>(this Exceptional<T1> @this, Func<T1, T2, T3, T4, R> f) =>
            @this.Map(f.CurryFirst());

        public static Exceptional<Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R>(this Exceptional<T1> @this, Func<T1, T2, T3, T4, T5, R> f) =>
            @this.Map(f.CurryFirst());

        // Monad

        public static Exceptional<R> FlatMap<T, R>(this Exceptional<T> @this, Func<T, Exceptional<R>> bind) =>
            @this.Match(
                Exception: ex => failure<R>(ex),
                Success: t => bind(t));

        // Applicative

        public static Exceptional<R> Apply<T, R>(this Exceptional<Func<T, R>> @this, Exceptional<T> arg) =>
            @this.Match(
                Exception: ex => failure<R>(ex),
                Success: func => arg.Map(func));

        public static Exceptional<Func<T2, R>> Apply<T1, T2, R>(this Exceptional<Func<T1, T2, R>> @this, Exceptional<T1> arg) =>
            @this.Map(FuncExtensions.Curry).Apply(arg);

        public static Exceptional<Func<T2, T3, R>> Apply<T1, T2, T3, R>(this Exceptional<Func<T1, T2, T3, R>> @this, Exceptional<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Exceptional<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>(this Exceptional<Func<T1, T2, T3, T4, R>> @this, Exceptional<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Exceptional<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>(this Exceptional<Func<T1, T2, T3, T4, T5, R>> @this, Exceptional<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static Exceptional<Unit> ForEach<T>(this Exceptional<T> @this, Action<T> f) => @this.Map(f.ToFunc());

        public static Unit Match<T>(this Exceptional<T> @this, Action<Exception> Exception, Action<T> Success) =>
            @this.Match(Exception.ToFunc(), Success.ToFunc());

        // TODO: Traversable

        // LINQ

        public static Exceptional<R> Select<T, R>(this Exceptional<T> @this, Func<T, R> map) => @this.Map(map);

        public static Exceptional<R> SelectMany<T, R>(this Exceptional<T> @this, Func<T, Exceptional<R>> bind) => @this.FlatMap(bind);

        public static Exceptional<S> SelectMany<T, R, S>(this Exceptional<T> @this, Func<T, Exceptional<R>> bind, Func<T, R, S> project) =>
            @this.FlatMap(t => bind(t).Map(r => project(t, r)));
    }
}
