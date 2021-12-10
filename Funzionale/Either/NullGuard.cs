using System.Diagnostics.CodeAnalysis;

namespace Funzionale.Either
{
    internal readonly struct NullGuard<T>
    {
        [DoesNotReturn]
        internal static T NullStateGuard(string state) =>
            throw new ArgumentNullException($"{state} cannot wrap null. Use Option instead.");

        [DoesNotReturn]
        internal static T NullInitGuard(string state) =>
            throw new ArgumentNullException($"Cannot initialize Either in the {state} state with a null {state} value. Use Option instead.");

        [DoesNotReturn]
        internal static T nullReturnGuard(string fName) =>
            throw new ArgumentNullException($"'{fName}' returned null. Consider returning an Option.");
    }
}
