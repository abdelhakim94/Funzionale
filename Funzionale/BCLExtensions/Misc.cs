namespace Funzionale
{
    public static class Misc
    {
        public static R Transform<T, R>(this T @this, Func<T, R> transform) => transform(@this);

        public static T Tee<T>(this T @this, Action<T> action)
        {
            action(@this);
            return @this;
        }
    }
}
