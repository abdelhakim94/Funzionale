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

        /// <summary>
        /// IEnumerable to Reader traverse with the monadic flow.
        /// </summary>
        public static Reader<Env, IEnumerable<R>> TraverseM<Env, T, R>(this IEnumerable<T> @this, Func<T, Reader<Env, R>> f)
            where Env : struct =>
                @this.Aggregate(
                    seed: reader<Env, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (readRs, t) => from rs in readRs
                                         from r in f(t)
                                         select rs.Append(r));

        /// <summary>
        /// IEnumerable to Reader traverse with the applicative flow.
        /// </summary>
        public static Reader<Env, IEnumerable<R>> TraverseA<Env, T, R>(this IEnumerable<T> @this, Func<T, Reader<Env, R>> f)
            where Env : struct =>
                @this.Aggregate(
                    seed: reader<Env, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (readRs, t) => reader<Env, Func<IEnumerable<R>, R, IEnumerable<R>>>(Enumerable.Append<R>)
                                            .Apply(readRs)
                                            .Apply(f(t)));

        /// <summary>
        /// IEnumerable to ReaderAsync traverse with the monadic flow.
        /// </summary>
        public static ReaderAsync<Env, IEnumerable<R>> TraverseM<Env, T, R>(this IEnumerable<T> @this, Func<T, ReaderAsync<Env, R>> f)
            where Env : struct =>
                @this.Aggregate(
                    seed: readerAsync<Env, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (readRs, t) => from rs in readRs
                                         from r in f(t)
                                         select rs.Append(r));

        /// <summary>
        /// IEnumerable to ReaderAsync traverse with the applicative flow.
        /// </summary>
        public static ReaderAsync<Env, IEnumerable<R>> TraverseA<Env, T, R>(this IEnumerable<T> @this, Func<T, ReaderAsync<Env, R>> f)
            where Env : struct =>
                @this.Aggregate(
                    seed: readerAsync<Env, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (readRs, t) => readerAsync<Env, Func<IEnumerable<R>, R, IEnumerable<R>>>(Enumerable.Append<R>)
                                            .Apply(readRs)
                                            .Apply(f(t)));

        /// <summary>
        /// IEnumerable to Writer traverse with the monadic flow.
        /// </summary>
        public static Writer<MonoidW, W, IEnumerable<R>> TraverseM<MonoidW, W, T, R>(this IEnumerable<T> @this, Func<T, Writer<MonoidW, W, R>> f)
            where MonoidW : struct, Monoid<W> =>
                @this.Aggregate(
                    seed: writer<MonoidW, W, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (writRs, t) => from rs in writRs
                                         from r in f(t)
                                         select rs.Append(r));

        /// <summary>
        /// IEnumerable to Writer traverse with the applicative flow.
        /// </summary>
        public static Writer<MonoidW, W, IEnumerable<R>> TraverseA<MonoidW, W, T, R>(this IEnumerable<T> @this, Func<T, Writer<MonoidW, W, R>> f)
            where MonoidW : struct, Monoid<W> =>
                @this.Aggregate(
                    seed: writer<MonoidW, W, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (writRs, t) => writer<MonoidW, W, Func<IEnumerable<R>, R, IEnumerable<R>>>(Enumerable.Append<R>)
                                            .Apply(writRs)
                                            .Apply(f(t)));

        /// <summary>
        /// IEnumerable to WriterAsync traverse with the monadic flow.
        /// </summary>
        public static WriterAsync<MonoidW, W, IEnumerable<R>> TraverseM<MonoidW, W, T, R>(this IEnumerable<T> @this, Func<T, WriterAsync<MonoidW, W, R>> f)
            where MonoidW : struct, Monoid<W> =>
                @this.Aggregate(
                    seed: writerAsync<MonoidW, W, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (writRs, t) => from rs in writRs
                                         from r in f(t)
                                         select rs.Append(r));

        /// <summary>
        /// IEnumerable to WriterAsync traverse with the applicative flow.
        /// </summary>
        public static WriterAsync<MonoidW, W, IEnumerable<R>> TraverseA<MonoidW, W, T, R>(this IEnumerable<T> @this, Func<T, WriterAsync<MonoidW, W, R>> f)
            where MonoidW : struct, Monoid<W> =>
                @this.Aggregate(
                    seed: writerAsync<MonoidW, W, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (writRs, t) => writerAsync<MonoidW, W, Func<IEnumerable<R>, R, IEnumerable<R>>>(Enumerable.Append<R>)
                                            .Apply(writRs)
                                            .Apply(f(t)));

        /// <summary>
        /// IEnumerable to State traverse with the monadic flow.
        /// </summary>
        public static State<S, IEnumerable<R>> TraverseM<S, T, R>(this IEnumerable<T> @this, Func<T, State<S, R>> f) =>
                @this.Aggregate(
                    seed: state<S, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (statRs, t) => from rs in statRs
                                         from r in f(t)
                                         select rs.Append(r));

        /// <summary>
        /// IEnumerable to State traverse with the applicative flow.
        /// </summary>
        public static State<S, IEnumerable<R>> TraverseA<S, T, R>(this IEnumerable<T> @this, Func<T, State<S, R>> f) =>
                @this.Aggregate(
                    seed: state<S, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (statRs, t) => state<S, Func<IEnumerable<R>, R, IEnumerable<R>>>(Enumerable.Append<R>)
                                            .Apply(statRs)
                                            .Apply(f(t)));

        /// <summary>
        /// IEnumerable to StateAsync traverse with the monadic flow.
        /// </summary>
        public static StateAsync<S, IEnumerable<R>> TraverseM<S, T, R>(this IEnumerable<T> @this, Func<T, StateAsync<S, R>> f) =>
                @this.Aggregate(
                    seed: stateAsync<S, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (statRs, t) => from rs in statRs
                                         from r in f(t)
                                         select rs.Append(r));

        /// <summary>
        /// IEnumerable to StateAsync traverse with the applicative flow.
        /// </summary>
        public static StateAsync<S, IEnumerable<R>> TraverseA<S, T, R>(this IEnumerable<T> @this, Func<T, StateAsync<S, R>> f) =>
                @this.Aggregate(
                    seed: stateAsync<S, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (statRs, t) => stateAsync<S, Func<IEnumerable<R>, R, IEnumerable<R>>>(Enumerable.Append<R>)
                                            .Apply(statRs)
                                            .Apply(f(t)));

        /// <summary>
        /// IEnumerable to Rws traverse with the monadic flow.
        /// </summary>
        public static Rws<MonoidW, Env, W, S, IEnumerable<R>> TraverseM<MonoidW, Env, W, S, T, R>(this IEnumerable<T> @this, Func<T, Rws<MonoidW, Env, W, S, R>> f)
           where Env : struct
           where MonoidW : struct, Monoid<W> =>
               @this.Aggregate(
                    seed: rws<MonoidW, Env, W, S, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (rwsRs, t) => from rs in rwsRs
                                        from r in f(t)
                                        select rs.Append(r));

        /// <summary>
        /// IEnumerable to Rws traverse with the applicative flow.
        /// </summary>
        public static Rws<MonoidW, Env, W, S, IEnumerable<R>> TraverseA<MonoidW, Env, W, S, T, R>(this IEnumerable<T> @this, Func<T, Rws<MonoidW, Env, W, S, R>> f)
            where Env : struct
            where MonoidW : struct, Monoid<W> =>
                @this.Aggregate(
                    seed: rws<MonoidW, Env, W, S, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (rwsRs, t) => rws<MonoidW, Env, W, S, Func<IEnumerable<R>, R, IEnumerable<R>>>(Enumerable.Append<R>)
                                            .Apply(rwsRs)
                                            .Apply(f(t)));

        /// <summary>
        /// IEnumerable to RwsAsync traverse with the monadic flow.
        /// </summary>
        public static RwsAsync<MonoidW, Env, W, S, IEnumerable<R>> TraverseM<MonoidW, Env, W, S, T, R>(this IEnumerable<T> @this, Func<T, RwsAsync<MonoidW, Env, W, S, R>> f)
           where Env : struct
           where MonoidW : struct, Monoid<W> =>
               @this.Aggregate(
                    seed: rwsAsync<MonoidW, Env, W, S, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (rwsRs, t) => from rs in rwsRs
                                        from r in f(t)
                                        select rs.Append(r));

        /// <summary>
        /// IEnumerable to RwsAsync traverse with the applicative flow.
        /// </summary>
        public static RwsAsync<MonoidW, Env, W, S, IEnumerable<R>> TraverseA<MonoidW, Env, W, S, T, R>(this IEnumerable<T> @this, Func<T, RwsAsync<MonoidW, Env, W, S, R>> f)
            where Env : struct
            where MonoidW : struct, Monoid<W> =>
                @this.Aggregate(
                    seed: rwsAsync<MonoidW, Env, W, S, IEnumerable<R>>(Enumerable.Empty<R>()),
                    func: (rwsRs, t) => rwsAsync<MonoidW, Env, W, S, Func<IEnumerable<R>, R, IEnumerable<R>>>(Enumerable.Append<R>)
                                            .Apply(rwsRs)
                                            .Apply(f(t)));
    }
}
