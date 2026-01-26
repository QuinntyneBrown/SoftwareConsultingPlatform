using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Core.Extensions;

public static class StringExtensions
{
    public static string ToSlug(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        // Convert to lowercase
        value = value.ToLowerInvariant();

        // Remove diacritics
        value = RemoveDiacritics(value);

        // Replace spaces with hyphens
        value = Regex.Replace(value, @"\s+", "-");

        // Remove invalid characters
        value = Regex.Replace(value, @"[^a-z0-9\-]", "");

        // Remove multiple consecutive hyphens
        value = Regex.Replace(value, @"-+", "-");

        // Trim hyphens from start and end
        value = value.Trim('-');

        return value;
    }

    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value))
            return value;

        if (value.Length <= maxLength)
            return value;

        return value[..(maxLength - suffix.Length)] + suffix;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
