using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public static class EitherExtensions
    {
        /// <summary>
        /// We could have let the null value returned by a function be lifted with the left/right
        /// functions, and that would have thrown an exception anyway. But we want to give
        /// the caller a clear indication that the problem comes from the fact that the function
        /// he provided returns null and that he should use Option instead.
        /// </summary>
        [DoesNotReturn]
        private static T Guard<T>(string fName) => Either.NullGuard<T>.nullReturnGuard(fName);

        // Functor

        public static Either<L, RR> Map<L, R, RR>(this Either<L, R> @this, Func<R, RR> map) =>
            @this.Match<Either<L, RR>>(
                Left: l => left(l!),
                Right: r => right(map(r) ?? Guard<RR>(nameof(map))));

        public static Either<LL, RR> Map<L, LL, R, RR>(this Either<L, R> @this, Func<L, LL> mapLeft, Func<R, RR> mapRight) =>
            @this.Match<Either<LL, RR>>(
                Left: l => left(mapLeft(l) ?? Guard<LL>(nameof(mapLeft))),
                Right: r => right(mapRight(r) ?? Guard<RR>(nameof(mapRight))));

        // Monad

        public static Either<L, RR> Bind<L, R, RR>(this Either<L, R> @this, Func<R, Either<L, RR>> bind) =>
            @this.Match(
                Left: l => left(l!),
                Right: r => bind(r));

        // Applicative

        public static Either<L, RR> Apply<L, R, RR>(this Either<L, Func<R, RR>> @this, Either<L, R> arg) =>
            @this.Match(
                Left: l => left(l!),
                Right: innerFunc => arg.Map(innerFunc));

        public static Either<L, Func<T2, R>> Apply<L, T1, T2, R>(this Either<L, Func<T1, T2, R>> @this, Either<L, T1> arg) =>
            Apply(@this.Map(FuncExtensions.Curry), arg);

        public static Either<L, Func<T2, T3, R>> Apply<L, T1, T2, T3, R>(this Either<L, Func<T1, T2, T3, R>> @this, Either<L, T1> arg) =>
            Apply(@this.Map(FuncExtensions.CurryFirst), arg);

        public static Either<L, Func<T2, T3, T4, R>> Apply<L, T1, T2, T3, T4, R>(this Either<L, Func<T1, T2, T3, T4, R>> @this, Either<L, T1> arg) =>
            Apply(@this.Map(FuncExtensions.CurryFirst), arg);

        public static Either<L, Func<T2, T3, T4, T5, R>> Apply<L, T1, T2, T3, T4, T5, R>(this Either<L, Func<T1, T2, T3, T4, T5, R>> @this, Either<L, T1> arg) =>
            Apply(@this.Map(FuncExtensions.CurryFirst), arg);

        // TODO: Make either traversable with other common monads like Task, IEnumerable...etc.

        // Utilities

        public static Either<L, Unit> ForEach<L, R>(this Either<L, R> @this, Action<R> action) =>
            @this.Map(action.ToFunc());

        public static Unit Match<L, R>(this Either<L, R> @this, Action<L> Left, Action<R> Right) =>
            @this.Match(Left.ToFunc(), Right.ToFunc());

        // LINQ

        public static Either<L, RR> Select<L, R, RR>(this Either<L, R> @this, Func<R, RR> map) => @this.Map(map);

        public static Either<L, RR> SelectMany<L, R, RR>(this Either<L, R> @this, Func<R, Either<L, RR>> bind) => @this.Bind(bind);

        public static Either<L, RRR> SelectMany<L, R, RR, RRR>(this Either<L, R> @this, Func<R, Either<L, RR>> bind, Func<R, RR, RRR> project) =>
            @this.Bind(r => bind(r).Map(rr => project(r, rr)));
    }
}
