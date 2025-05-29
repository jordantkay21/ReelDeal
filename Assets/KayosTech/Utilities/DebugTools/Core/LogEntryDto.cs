using System;

namespace KayosTech.Utilities.DebugTools
{
    /// <summary>
    /// Represents a single log entry with metadata and formatting logic.
    /// </summary>
    public struct LogEntryDto
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
            return $"[<b>{Tag}</b>] [{Level}] [{ScriptName}] [{MethodName}]" +
                                   $"\n {Timestamp:HH:mm:ss.fff tt}" +
                                   $"\n <b>{Message}</b>";
        }
    }
}
