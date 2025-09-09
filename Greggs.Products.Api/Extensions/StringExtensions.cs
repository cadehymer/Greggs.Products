using System;

namespace Greggs.Products.Api.Extensions;

public static class StringExtensions
{
    public static string IfNullOrEmpty(this string value, string defaultValue)
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        return value;
    }

    public static string IfNullOrEmpty(this string value, Func<string> defaultValue)
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue();
        }

        return value;
    }
}