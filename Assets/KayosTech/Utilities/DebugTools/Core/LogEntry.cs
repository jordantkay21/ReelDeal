using System;

namespace KayosTech.Utilities.DebugTools
{
    /// <summary>
    /// Represents a single log entry with metadata and formatting logic.
    /// </summary>
    public struct LogEntry
    {
        public string ScriptName;
        public string MethodName;
        public LogLevel Level;
        public float DisplayTime;
        public string Tag;
        public string Message;
        public DateTime Timestamp;

        public override string ToString()
        {
            return $"[{ScriptName}] [{MethodName}]" +
                                   $"\n [{Level}] [{Tag}]" +
                                   $"\n {Timestamp:HH:mm:ss.fff tt}" +
                                   $"\n <b>{Message}</b>";
        }
    }
}
