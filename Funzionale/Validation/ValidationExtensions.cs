using System.Diagnostics.CodeAnalysis;

namespace Funzionale.Validation
{
    using static Prelude;

    public static class ValidationExtensions
    {
        [DoesNotReturn]
        private static T Guard<T>(string fName) => throw new ArgumentNullException(fName, $"{fName} returned a null. Consider returning an Option.");

        // Functor

        public static Validation<R> Map<T, R>(this Validation<T> @this, Func<T, R> map) =>
            @this.Match(
                Invalid: e => invalid(e),
                Valid: t => valid(map(t) ?? Guard<R>(nameof(map))));

        public static Validation<Func<T2, R>> Map<T1, T2, R>(this Validation<T1> @this, Func<T1, T2, R> f) =>
            @this.Map(f.Curry());

        public static Validation<Func<T2, T3, R>> Map<T1, T2, T3, R>(this Validation<T1> @this, Func<T1, T2, T3, R> f) =>
            @this.Map(f.CurryFirst());

        public static Validation<Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R>(this Validation<T1> @this, Func<T1, T2, T3, T4, R> f) =>
            @this.Map(f.CurryFirst());

        public static Validation<Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R>(this Validation<T1> @this, Func<T1, T2, T3, T4, T5, R> f) =>
            @this.Map(f.CurryFirst());

        // Monad

        public static Validation<R> Bind<T, R>(this Validation<T> @this, Func<T, Validation<R>> bind) =>
            @this.Match(
                Invalid: e => invalid(e),
                Valid: t => bind(t));

        // Applicative

        public static Validation<R> Apply<T1, R>(this Validation<Func<T1, R>> @this, Validation<T1> arg) =>
            @this.Match(
                Valid: func => arg.Match(
                    Valid: t => valid(func(t) ?? Guard<R>(nameof(func))),
                    Invalid: errArg => invalid(errArg)),
                Invalid: errFunc => arg.Match(
                    Valid: _ => invalid(errFunc),
                    Invalid: errArg => invalid(errFunc.Concat(errArg))));

        public static Validation<Func<T2, R>> Apply<T1, T2, R>(this Validation<Func<T1, T2, R>> @this, Validation<T1> arg) =>
            @this.Map(FuncExtensions.Curry).Apply(arg);

        public static Validation<Func<T2, T3, R>> Apply<T1, T2, T3, R>(this Validation<Func<T1, T2, T3, R>> @this, Validation<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Validation<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>(this Validation<Func<T1, T2, T3, T4, R>> @this, Validation<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Validation<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>(this Validation<Func<T1, T2, T3, T4, T5, R>> @this, Validation<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities
        public static Validation<Unit> Foreach<T>(this Validation<T> @this, Action<T> action) => @this.Map(action.ToFunc());
        public static Unit Match<T, R>(this Validation<T> @this, Action<IEnumerable<Error>> Invalid, Action<T> Valid) =>
            @this.Match(Invalid.ToFunc(), Valid.ToFunc());

        // TODO: Traversable

        // LINQ

        public static Validation<R> Select<T, R>(this Validation<T> @this, Func<T, R> map) => @this.Map(map);

        public static Validation<R> SelectMany<T, R>(this Validation<T> @this, Func<T, Validation<R>> bind) =>
            @this.Bind(bind);

        public static Validation<S> SelectMany<T, R, S>(this Validation<T> @this, Func<T, Validation<R>> bind, Func<T, R, S> project) =>
            @this.Bind(t => bind(t).Map(r => project(t, r)));
    }
}
