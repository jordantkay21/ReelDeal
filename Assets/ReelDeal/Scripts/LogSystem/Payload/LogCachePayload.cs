using System;
using KayosTech.ReelDeal.Prototype.LogSystem;
using UnityEngine;

public class LogCachePayload : IResponsePayload
{
    public DataFlow dataFlow => DataFlow.Cache;
    public AppLogType Type { get; }
    public string Formatted { get; }

    public LogCachePayload(AppLogType type, string tag, string message, string callerScript, string callerMethod, string sourceSystem, DateTime timestamp, Color color)
    {

        string prefix = $"[{sourceSystem} [{tag}]";
        string caller = $"[{timestamp:HH:mm:ss}] [{callerScript}.{callerMethod}]";
        string textColor = Utilities.ColorToHex(color);

        string typeIndicator = type switch
        {
            AppLogType.Internal => null,
            AppLogType.Info => " ℹ️ℹ️ℹ️ ",
            AppLogType.Success => " ✅✅✅ ",
            AppLogType.Alert => " ⚠️⚠️⚠️ ",
            AppLogType.Error => " ❎❎❎ ",
            AppLogType.Urgent => " 📢📢📢 "
        };

        Type = type;
        Formatted = $"<color=#{textColor}>{prefix}" + 
                    $"\n {message}" +
                    $"\n {caller}</color>";
    }

}


