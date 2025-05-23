using UnityEngine;

namespace KayosTech.Utilities.DebugTools
{
    public static class LogUtility
    {
        public static string GetColorHex(LogLevel level)
        {
            return level switch
            {
                LogLevel.Info => "#4FC3F7",
                LogLevel.Success => "#32D475",
                LogLevel.Alert => "#FFB347",
                LogLevel.Error => "#E94F4F",
                _ => "#CCCCCC"
            };
        }
    }
}
