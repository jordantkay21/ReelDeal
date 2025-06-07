using System;
using KayosTech.ReelDeal.Prototype.LogSystem;
using UnityEngine;

public class LogCachePayload : IResponsePayload
{
    public DataFlow DataFlow => DataFlow.Cache;
    public AppLogType Type { get; }
    public string ConsoleFormat { get; }
    public string CacheFormat { get; }

    public LogCachePayload(AppLogType type, string tag, string message, string callerScript, string callerMethod, string sourceSystem, DateTime timestamp, Color color)
    {

        string prefix = $"[{sourceSystem}] [{tag}]";
        string caller = $"[{timestamp:hh:mm:ss.fff tt}] [{callerScript}.{callerMethod}]";
        string textColor = Utilities.ColorToHex(color);

        string typeIndicator = type switch
        {
            AppLogType.Internal => " ⚙️⚙️⚙️ ",
            AppLogType.Info => " ℹ️ℹ️ℹ️ ",
            AppLogType.Success => " ✅✅✅ ",
            AppLogType.Alert => " ⚠️⚠️⚠️ ",
            AppLogType.Error => " 🛑🛑🛑 ",
            AppLogType.Urgent => " 📢📢📢 ",
            _ => null
        };

        Type = type;
        ConsoleFormat = $"{typeIndicator} <color=#{textColor}>{prefix}" + 
                    $"\n     {caller}" +
                    $"\n     <b>{message}</b></color>";
        
        CacheFormat = $"{typeIndicator} {prefix}" +
                      $"\n     {caller}" +
                      $"\n     {message}";
    }

}


