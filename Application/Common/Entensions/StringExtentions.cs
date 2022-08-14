namespace Application.Common.Entensions;

public static class StringExtentions
{
    public static string NormalizeName(this string str)
    {
        return str.Trim().ToLower();
    }

    public static string MultipleReplace(this string str, IDictionary<string, string> replaceWords)
    {
        var response = string.Empty;

        foreach (var temp in replaceWords)
            response = str.Replace(temp.Key, temp.Value);

        return response;
    }
}
