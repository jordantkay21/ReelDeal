using KayosTech.ReelDeal.Prototype.LogSystem.Settings;
using System;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem.DataStructure
{
    public class LogCommandDTO
    {
        public bool ShowInUI { get; }
        public AppLogType Type { get; }
        public string Message { get; }
        public string Tag { get; }
        public string SourceSystem { get; }
        public string CallerScript { get; }
        public string CallerMethod { get; }
       public DateTime Timestamp { get; }
       public Color PrimaryColor { get; }
       public Color SecondaryColor { get; }
       public float Duration { get; }
       public float FadeDuration { get; }

       public LogCommandDTO(AppLogType type, string tag, string message, string callerScript, string callerMethod, string sourceSystem, DateTime timestamp, Color primaryColor, Color secondaryColor, float duration, float fadeDuration, bool showInUI)
       {
           Type = type;
           Tag = tag;
           Message = message;
           CallerScript = callerScript;
           CallerMethod = callerMethod;
           SourceSystem = sourceSystem;
           Timestamp = timestamp;
           PrimaryColor = primaryColor;
           SecondaryColor = secondaryColor;
           Duration = duration;
           FadeDuration = fadeDuration;
           ShowInUI = showInUI;
       }
    }

    
}