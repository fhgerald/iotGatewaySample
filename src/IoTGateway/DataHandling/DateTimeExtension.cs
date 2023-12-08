namespace IoTGateway.DataHandling;

internal static class DateTimeExtensions
{
    public static long ToUnixEpochInMilliSecondsTime(this DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
            return 0;
        DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan timeSpan = dateTime - dateTime1;
        return timeSpan.TotalMilliseconds >= 0.0 ? (long) timeSpan.TotalMilliseconds : throw new ArgumentOutOfRangeException("Unix epoc starts January 1st, 1970");
    }
}