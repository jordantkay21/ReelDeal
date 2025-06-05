using System;
using KayosTech.ReelDeal.Prototype.LogSystem.Settings;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem.Payload
{
    public class LogActionPayload  : IActionPayload
    {
        public AppLogType type { get; }
        public string tag { get; }
        public string message { get; }
        public string callerScript { get; }
        public string callerMethod { get; }
        public string sourceSystem { get; }
        public DateTime timestamp { get; }


        public LogActionPayload(AppLogType type, string tag, string message, string callerScript, string callerMethod, string sourceSystem, DateTime timestamp)
        {
            this.type = type;
            this.tag = tag;
            this.message = message;
            this.callerScript = callerScript;
            this.callerMethod = callerMethod; 
            this.sourceSystem = sourceSystem;
            this.timestamp = timestamp;
        }
    }
}