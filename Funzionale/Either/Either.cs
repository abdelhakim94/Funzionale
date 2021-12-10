using System.Diagnostics.CodeAnalysis;

namespace Funzionale
{
    using static Prelude;

    public partial class Prelude
    {
        public static Either.Left<L> left<L>([DisallowNull] L l) =>
            new(l ?? throw new ArgumentNullException(nameof(l), "'Left' cannot wrap null. Use Option instead"));

        public static Either.Right<R> right<R>([DisallowNull] R r) =>
            new(r ?? throw new ArgumentNullException(nameof(r), "'Right' cannot wrap null. Use Option instead"));
    }

    namespace Either
    {
        public readonly struct Left<L>
        {
            internal readonly L Value;
            internal Left([DisallowNull] L value)
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value), "'Left' cannot wrap null. Use Option instead.");
                Value = value;
            }
        }

        public readonly struct Right<R>
        {
            internal readonly R Value;
            internal Right([DisallowNull] R value)
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value), "'Right' cannot wrap null. Use Option instead");
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
        readonly bool isLeft{ get; }

        private Either([DisallowNull] L l)
        {
            if (l is null)
                throw new ArgumentNullException(nameof(l), "Cannot initialize Either in the Left state with a null left value. Use Option instead.");
            left = l;
            right = default;
            isLeft = true;
        }

        private Either([DisallowNull] R r)
        {
            if (r is null)
                throw new ArgumentNullException(nameof(r), "Cannot initialize Either in the Right state with a null right value. Use Option instead.");
            left = default;
            right = r;
            isLeft = false;
        }

        public static implicit operator Either<L, R>(L l) =>
            new(l ?? throw new ArgumentNullException(nameof(l), "Cannot initialize Either in the Left state with a null left value. Use Option instead."));
        public static implicit operator Either<L, R>(R r) =>
            new(r ?? throw new ArgumentNullException(nameof(r), "Cannot initialize Either in the Right state with a null right value. Use Option instead."));

        public static implicit operator Either<L, R>(Either.Left<L> l) =>
            new(l.Value ?? throw new ArgumentNullException(nameof(l), "Cannot initialize Either in the Left state with a null left value. Use Option instead."));

        public static implicit operator Either<L, R>(Either.Right<R> r) =>
            new(r.Value ?? throw new ArgumentNullException(nameof(r), "Cannot initialize Either in the Right state with a null right value. Use Option instead."));

        public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right) => isLeft ? Left(left) : Right(right);
    }
}
