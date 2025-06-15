using System;
using KayosTech.ReelDeal.Prototype.LogSystem.Settings;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem.DataStructure
{
    public class LogActionPayload 
    {
        public AppLogType Type { get; }
        public string Tag { get; }
        public string Message { get; }
        public string CallerScript { get; }
        public string CallerMethod { get; }
        public string SourceSystem { get; }
        public DateTime Timestamp { get; }


        public LogActionPayload(AppLogType type, string tag, string message, string callerScript, string callerMethod, string sourceSystem, DateTime timestamp)
        {
            this.Type = type;
            this.Tag = tag;
            this.Message = message;
            this.CallerScript = callerScript;
            this.CallerMethod = callerMethod; 
            this.SourceSystem = sourceSystem;
            this.Timestamp = timestamp;
        }
    }
}