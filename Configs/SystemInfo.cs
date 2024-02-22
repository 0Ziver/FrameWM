using Frame.Time;

namespace Frame.Settings
{
    public struct SystemInfo
    {
        public TimeFormat UserTimeFormat => LocalTime.GetTimeFormat();
    }
}