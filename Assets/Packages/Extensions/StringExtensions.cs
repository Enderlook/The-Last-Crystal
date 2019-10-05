public static class StringExtensions
{
    /// <summary>
    /// Return an string which first character is to upper case.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string FirstCharToUpper(this string source)
    {
        if (source == null) throw new System.ArgumentNullException(nameof(source));

        return source.Length > 1 ? char.ToUpper(source[0]) + source.Substring(1) : source.ToUpper();
    }
}
