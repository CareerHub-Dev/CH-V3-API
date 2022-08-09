namespace Application.Common.Entensions;

public static class StringExtentions
{
    public static string NormalizeName(this string str)
    {
        return str.Trim().ToLower();
    }
}
