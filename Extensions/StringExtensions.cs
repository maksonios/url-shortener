namespace UrlShortener.Extensions;

public static class StringExtensions
{
    public static Guid ToGuid(this string value)
    {
        if (Guid.TryParse(value, out Guid result))
        {
            return result;
        }

        throw new ArgumentException("The input string is not a valid GUID.");
    }
}