namespace Funzionale
{
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

        public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> @this, T1 t1) =>
            t2 => @this(t1, t2);
        
        public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> @this, T1 t1) =>
            (t2, t3) => @this(t1, t2, t3);

        public static Func<T2, T3, T4, R> Apply<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> @this, T1 t1) =>
            (t2, t3, t4) => @this(t1, t2, t3, t4);

        public static Func<T2, T3, T4, T5, R> Apply<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> @this, T1 t1) =>
            (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);
    }
}
