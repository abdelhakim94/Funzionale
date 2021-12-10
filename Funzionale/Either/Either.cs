using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public partial class Prelude
    {
        public static Either.Left<L> left<L>([DisallowNull][NotNull] L l) =>
            new(l ?? Either.NullGuard<L>.NullStateGuard("Left"));

        public static Either.Right<R> right<R>([DisallowNull][NotNull] R r) =>
            new(r ?? Either.NullGuard<R>.NullStateGuard("Right"));
    }

    namespace Either
    {
        public readonly struct Left<L>
        {
            internal readonly L Value;
            internal Left([DisallowNull][NotNull] L value)
            {
                if (value is null) NullGuard<L>.NullStateGuard("Left");
                Value = value;
            }
        }

        public readonly struct Right<R>
        {
            internal readonly R Value;
            internal Right([DisallowNull][NotNull] R value)
            {
                if (value is null) NullGuard<R>.NullStateGuard("Right");
                Value = value;
            }
        }
    }

    public readonly struct Either<L, R>
    {
        readonly L? left;
        readonly R? right;

        [MemberNotNullWhen(true, nameof(left))]
        [MemberNotNullWhen(false, nameof(right))]
        readonly bool isLeft { get; }

        [DoesNotReturn]
        private static T Guard<T>(string state) => Either.NullGuard<T>.NullInitGuard(state);

        private Either([DisallowNull][NotNull] L l)
        {
            if (l is null) Guard<L>("Left");
            left = l;
            right = default;
            isLeft = true;
        }

        private Either([DisallowNull][NotNull] R r)
        {
            if (r is null) Guard<R>("Right");
            left = default;
            right = r;
            isLeft = false;
        }

        public static implicit operator Either<L, R>([DisallowNull][NotNull] L l) => new(l ?? Guard<L>("Left"));
        public static implicit operator Either<L, R>([DisallowNull][NotNull] R r) => new(r ?? Guard<R>("Right"));
        public static implicit operator Either<L, R>(Either.Left<L> l) => new(l.Value ?? Guard<L>("Left"));
        public static implicit operator Either<L, R>(Either.Right<R> r) => new(r.Value ?? Guard<R>("Right"));
        public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right) => isLeft ? Left(left) : Right(right);
    }
}
