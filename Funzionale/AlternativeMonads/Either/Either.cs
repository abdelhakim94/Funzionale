using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public partial class Prelude
    {
        public static Either.Left<L> left<L>([DisallowNull][NotNull] L l) => new(l);

        public static Either.Right<R> right<R>([DisallowNull][NotNull] R r) => new(r);
    }

    namespace Either
    {
        public readonly struct Left<L>
        {
            internal readonly L Value;
            internal Left([DisallowNull][NotNull] L value)
            {
                Value = value ?? NullGuard<L>.NullStateGuard("Left");
            }
        }

        public readonly struct Right<R>
        {
            internal readonly R Value;
            internal Right([DisallowNull][NotNull] R value)
            {
                Value = value ?? NullGuard<R>.NullStateGuard("Right");
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
            left = l ?? Guard<L>("Left"); ;
            right = default;
            isLeft = true;
        }

        private Either([DisallowNull][NotNull] R r)
        {
            right = r ?? Guard<R>("Right");
            left = default;
            isLeft = false;
        }

        public static implicit operator Either<L, R>([DisallowNull][NotNull] L l) => new(l);
        public static implicit operator Either<L, R>([DisallowNull][NotNull] R r) => new(r);
        public static implicit operator Either<L, R>(Either.Left<L> l) => new(l.Value!);
        public static implicit operator Either<L, R>(Either.Right<R> r) => new(r.Value!);
        public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right) => isLeft ? Left(left) : Right(right);
    }
}
