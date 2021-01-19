
namespace System
{
    public static class SystemExtensions
    {
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }
        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }
        public static string ToUpperNormalized(this string text)
        {
            if (text.IsNullOrEmpty())
                return null;
            return text.Normalize().ToUpperInvariant();
        }
    }
}
