﻿namespace Funzionale
{
    using static Prelude;
    public static class OptionExtensions
    {
        // Functor

        public static Option<R> Map<T, R>(this Option.None _, Func<T, R> map) => none;
        public static Option<R> Map<T, R>(this Option.Some<T> s, Func<T, R> map) => map(s.value);
        public static Option<R> Map<T, R>(this Option<T> @this, Func<T, R> map) =>
            @this.Match(
                None: () => none,
                Some: v => some(map(v)));

        public static Option<Func<T2, R>> Map<T1, T2, R>(this Option<T1> @this, Func<T1, T2, R> f) =>
            Map(@this, f.CurryFirst());

        public static Option<Func<T2, T3, R>> Map<T1, T2, T3, R>(this Option<T1> @this, Func<T1, T2, T3, R> f) =>
            Map(@this, f.CurryFirst());

        public static Option<Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R>(this Option<T1> @this, Func<T1, T2, T3, T4, R> f) =>
            Map(@this, f.CurryFirst());

        public static Option<Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R>(this Option<T1> @this, Func<T1, T2, T3, T4, T5, R> f) =>
            Map(@this, f.CurryFirst());

        // Monad

        public static Option<R> Bind<T, R>(this Option<T> @this, Func<T, Option<R>> bind) =>
            @this.Match(
                None: () => none,
                Some: v => bind(v));

        // Applicative

        public static Option<R> Apply<T, R>(this Option<Func<T, R>> @this, Option<T> arg) =>
            @this.Match(
                None: () => none,
                Some: f => arg.Match(
                    None: () => none,
                    Some: a => some(f(a))));

        public static Option<Func<T2, R>> Apply<T1, T2, R>(this Option<Func<T1, T2, R>> @this, Option<T1> arg) =>
            @this.Map(FuncExtensions.Curry).Apply(arg);

        public static Option<Func<T2, T3, R>> Apply<T1, T2, T3, R>(this Option<Func<T1, T2, T3, R>> @this, Option<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Option<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>(this Option<Func<T1, T2, T3, T4, T5, R>> @this, Option<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        public static Option<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>(this Option<Func<T1, T2, T3, T4, R>> @this, Option<T1> arg) =>
            @this.Map(FuncExtensions.CurryFirst).Apply(arg);

        // Utilities

        public static Option<Unit> ForEach<T>(this Option<T> @this, Func<T, Unit> f) => Map(@this, f);
        public static Option<Unit> ForEach<T>(this Option<T> @this, Action<T> action) => ForEach(@this, action.ToFunc());

        public static Unit Match<T>(this Option<T> @this, Action None, Action<T> Some) =>
            @this.Match(None.ToFunc(), Some.ToFunc());

        // LINQ

        public static Option<R> Select<T, R>(this Option<T> @this, Func<T, R> map) => Map(@this, map);

        public static Option<R> SelectMany<T, R>(this Option<T> @this, Func<T, Option<R>> bind) => Bind(@this, bind);

        public static Option<S> SelectMany<T, R, S>(this Option<T> @this, Func<T, Option<R>> bind, Func<T, R, S> project) =>
            Bind(@this, x => Map(bind(x), y => project(x, y)));

        public static Option<T> Where<T>(this Option<T> @this, Func<T, bool> predicate) =>
            @this.Match(
                None: () => none,
                Some: v => predicate(v) ? some(v) : none);
    }
}