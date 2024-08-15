using System;

namespace Common
{
    public class Utils
    {
        public static string ToReadableString(TimeSpan span)
        {
            var formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? $"{span.Days:0}d, " : string.Empty,
                span.Duration().Hours > 0 || span.Duration().Days > 0
                    ? $"{span.Hours:00}:"
                    : string.Empty,
                $"{span.Minutes:00}:",
                $"{span.Seconds:00}");
            if (formatted.EndsWith(": ")) formatted = formatted.Substring(0, formatted.Length - 2);
            if (string.IsNullOrEmpty(formatted)) formatted = "0 s";
            return formatted;
        }
    }
}