using UnityEngine;

namespace KayosTech.Utilities.DebugTools
{
    public static class LogUtility
    {
        public static string GetColorHex(LogLevel level)
        {
            return level switch
            {
                LogLevel.Info => "#32B5E1",
                LogLevel.Success => "#32D475",
                LogLevel.Alert => "#FFB347",
                LogLevel.Error => "#E94F4F",
                LogLevel.ByPass => "#FF00FF",
                _ => "#CCCCCC"
            };
        }
    }
}
