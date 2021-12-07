namespace Funzionale
{
    using static Prelude;
    public static class ActionExtentions
    {
        // Action to Func

        public static Func<Unit> ToFunc(this Action @this) =>
            () => { @this(); return unit; };

        public static Func<T, Unit> ToFunc<T>(this Action<T> @this) =>
            (T t) => { @this(t); return unit; };

        public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> @this) =>
            (T1 t1, T2 t2) => { @this(t1, t2); return unit; };

        public static Func<T1, T2, T3, Unit> ToFunc<T1, T2, T3>(this Action<T1, T2, T3> @this) =>
             (T1 t1, T2 t2, T3 t3) => { @this(t1, t2, t3); return unit; };

        public static Func<T1, T2, T3, T4, Unit> ToFunc<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this) =>
            (T1 t1, T2 t2, T3 t3, T4 t4) => { @this(t1, t2, t3, t4); return unit; };

        public static Func<T1, T2, T3, T4, T5, Unit> ToFunc<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this) =>
            (T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) => { @this(t1, t2, t3, t4, t5); return unit; };
    }
}
