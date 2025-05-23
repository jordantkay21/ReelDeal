using System;
using UnityEngine;

namespace KayosTech.Utilities.DebugTools
{
    public struct LogEntry
    {
        public string ScriptName;
        public string MethodName;
        public LogLevel Level;
        public string Tag;
        public string Message;
        public DateTime Timestamp;

        public string Formatted => $"[{ScriptName}] [{MethodName}] | [{Level}] [{Tag}]" +
                                   $"\n {Timestamp:HH:mm:ss.fff tt} " +
                                   $"\n  <b>{Message}</b>";
    }
}
