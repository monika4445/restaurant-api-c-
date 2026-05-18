namespace RestaurantApi.Services;

public static class SearchHelpers
{
    public static string ToIlikePattern(string input)
    {
        var escaped = input.Trim()
            .Replace("\\", "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_");
        return $"%{escaped}%";
    }
}
