using System.Globalization;


namespace Frame.Time;

public enum TimeFormat
{
    _12h = 1,
    _24h = 2
}

public class LocalTime
{
    private static TimeFormat? _timeFormat;

    public LocalTime()
    {
        SetTimeFormat();
    }


    public static TimeFormat GetTimeFormat()
    {
        if (_timeFormat == null)
        {
            SetTimeFormat();
        }

        return (TimeFormat)_timeFormat!;
    }

    static void SetTimeFormat()
    {
        if (_timeFormat != null) return;
        CultureInfo culture = CultureInfo.CurrentCulture;
        DateTimeFormatInfo dtfi = culture.DateTimeFormat;

        string shortTimePattern = dtfi.ShortTimePattern;
        _timeFormat = shortTimePattern.Contains('H') ? TimeFormat._24h : TimeFormat._12h;
    }
}