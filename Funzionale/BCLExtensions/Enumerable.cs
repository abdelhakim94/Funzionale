namespace Funzionale
{
    using static Prelude;

    public static class EnumerableExtensions
    {
        // Traversable

        /// <summary>
        /// IEnumerable to Exceptional traverse with the monadic flow.
        /// </summary>
        public static Exceptional<IEnumerable<R>> TraverseM<T, R>(this IEnumerable<T> @this, Func<T, Exceptional<R>> f) =>
            @this.Aggregate(
                seed: success(Enumerable.Empty<R>()),
                func: (exRs, t) => from rs in exRs
                                   from r in f(t)
                                   select rs.Append(r));

        /// <summary>
        /// IEnumerable to Exceptional traverse with the applicative flow.
        /// </summary>
        public static Exceptional<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> @this, Func<T, Exceptional<R>> f) =>
            @this.Aggregate(
                seed: success(Enumerable.Empty<R>()),
                func: (exRs, t) => success(Enumerable.Append<R>)
                                    .Apply(exRs)
                                    .Apply(f(t)));

        /// <summary>
        /// IEnumerable to Option traverse with the monadic flow.
        /// </summary>
        public static Option<IEnumerable<R>> TraverseM<T, R>(this IEnumerable<T> @this, Func<T, Option<R>> f) =>
            @this.Aggregate(
                seed: some(Enumerable.Empty<R>()),
                func: (optRs, t) => from rs in optRs
                                    from r in f(t)
                                    select rs.Append(r));

        /// <summary>
        /// IEnumerable to Option traverse with the applicative flow.
        /// </summary>
        public static Option<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> @this, Func<T, Option<R>> f) =>
            @this.Aggregate(
                seed: some(Enumerable.Empty<R>()),
                func: (optRs, t) => some(Enumerable.Append<R>)
                                        .Apply(optRs)
                                        .Apply(f(t)));

        /// <summary>
        /// IEnumerable to Validation traverse with the monadic flow.
        /// </summary>
        public static Validation<IEnumerable<R>> TraverseM<T, R>(this IEnumerable<T> @this, Func<T, Validation<R>> f) =>
            @this.Aggregate(
                seed: valid(Enumerable.Empty<R>()),
                func: (valRs, t) => from rs in valRs
                                    from r in f(t)
                                    select rs.Append(r));

        /// <summary>
        /// IEnumerable to Validation traverse with the applicative flow.
        /// </summary>
        public static Validation<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> @this, Func<T, Validation<R>> f) =>
            @this.Aggregate(
                seed: valid(Enumerable.Empty<R>()),
                func: (valRs, t) => valid(Enumerable.Append<R>)
                                        .Apply(valRs)
                                        .Apply(f(t)));

        /// <summary>
        /// IEnumerable to Try traverse with the monadic flow.
        /// </summary>
        public static Try<IEnumerable<R>> TraverseM<T, R>(this IEnumerable<T> @this, Func<T, Try<R>> f) =>
            @this.Aggregate(
                seed: @try(Enumerable.Empty<R>()),
                func: (tryRs, t) => from rs in tryRs
                                    from r in f(t)
                                    select rs.Append(r));

        /// <summary>
        /// IEnumerable to Try traverse with the applicative flow.
        /// </summary>
        public static Try<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> @this, Func<T, Try<R>> f) =>
            @this.Aggregate(
                seed: @try(Enumerable.Empty<R>()),
                func: (tryRs, t) => @try(Enumerable.Append<R>)
                                        .Apply(tryRs)
                                        .Apply(f(t)));

        /// <summary>
        /// IEnumerable to Task traverse with the monadic flow. Runs the tasks in sequence.
        /// Use the applicative flow instead.
        /// </summary>
        public static Task<IEnumerable<R>> TraverseM<T, R>(this IEnumerable<T> @this, Func<T, Task<R>> f) =>
            @this.Aggregate(
                seed: async(Enumerable.Empty<R>()),
                func: (taskRs, t) => from rs in taskRs
                                     from r in f(t)
                                     select rs.Append(r));

        /// <summary>
        /// IEnumerable to Task traverse with the applicative flow. Runs the tasks in parallel.
        /// Faster than the monadic flow.
        /// </summary>
        public static Task<IEnumerable<R>> TraverseA<T, R>(this IEnumerable<T> @this, Func<T, Task<R>> f) =>
            @this.Aggregate(
                seed: async(Enumerable.Empty<R>()),
                func: (taskRs, t) => async(Enumerable.Append<R>)
                                        .Apply(taskRs)
                                        .Apply(f(t)));

    }
}
