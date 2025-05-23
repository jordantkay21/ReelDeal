using System;
using UnityEngine;
using System.Runtime .CompilerServices;

namespace KayosTech.Utilities.DebugTools
{

    [System.Serializable]
    public enum LogLevel
    {
        Internal, //Backend use only - to file/console
        Info, //Developer-readable, can show to player in dev builds
        Success, //Player-visible
        Alert, //Player-visible warning
        Error //Player-visible error
    }




    /// <summary>
    /// Captures log messages and routes them to both visual and persistent outputs
    /// </summary>
    public class LogRouter : MonoBehaviour
    {
        public static event Action<LogEntry> OnLogReceived;
        public static bool SuppressFrontendLogs { get; set; } = false;


        private void Awake()
        {
            LogFileWriter.InitializeLogFile();
        }

        private void OnApplicationQuit()
        {
            LogFileWriter.SaveLogFile();
        }

        public static void Log(string tag, string message, LogLevel level = LogLevel.Internal, [CallerMemberName] string callingMethod = "", [CallerFilePath] string callingFile = "")
        {
            string scriptName = System.IO.Path.GetFileNameWithoutExtension(callingFile);

            LogEntry entry = new LogEntry
            {
                ScriptName = scriptName,
                MethodName = callingMethod,
                Level = level,
                Tag = tag,
                Message = message, 
                Timestamp = DateTime.Now
            };

            string color = LogUtility.GetColorHex(entry.Level);
            string formatted = $"<color={color}>{entry.Formatted}</color>";

            //Console Logging
            switch (level)
            {
                case LogLevel.Internal:
                    Debug.Log($"{formatted}");
                    break;
                case LogLevel.Info:
                    Debug.Log($"{formatted}");
                    break;
                case LogLevel.Success:
                    Debug.Log($"{formatted}");
                    break;
                case LogLevel.Alert:
                    Debug.LogWarning($"{formatted}");
                    break;
                case LogLevel.Error:
                    Debug.LogError($"{formatted}");
                    break;
                default:
                    Debug.Log($"[UNKNOWN LEVEL] {formatted}");
                    break;
            }

            HandleFileLog(entry);

            if (!SuppressFrontendLogs)
                OnLogReceived?.Invoke(entry);
        }

        private static void HandleFileLog(LogEntry entry)
        {
            LogFileWriter.AppendToLog(entry.Formatted);
        }

    }
}
