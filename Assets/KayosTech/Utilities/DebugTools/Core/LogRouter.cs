using System;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace KayosTech.Utilities.DebugTools
{

    [System.Serializable]
    public enum LogLevel
    {
        Internal, //Backend use only - to file/console
        Info, //Developer-readable, can show to player in dev builds
        Success, //Player-visible
        Alert, //Player-visible warning
        Error, //Player-visible error
        ByPass //Logs that push to UI no matter user settings
    }




    /// <summary>
    /// Captures log messages and routes them to both visual and persistent outputs
    /// </summary>
    public class LogRouter : MonoBehaviour
    {

        public static event Action<LogEntryDto> OnLogReceived;
        public static bool SuppressFrontendLogs = false;
        public static bool SaveToFile = true;

        private void Awake()
        {
            LogFileWriter.InitializeLogFile();
            LogTemp("Temp Log Test");
        }

        private void OnApplicationQuit()
        {
            if(SaveToFile)
                LogFileWriter.SaveLogFile();
        }

        public static void ClearLogCache() => LogFileWriter.ClearAllLogs();

        /// <summary>
        /// Logs a Message with the specified Tag and level.
        /// </summary>
        /// <param name="tag">Short context descriptor (e.g., SYSTEM, NETWORK)</param>
        /// <param name="message">The body of the log Message</param>
        /// <param name="level">Severity level of the log</param>
        /// <param name="callingMethod">Auto filled method name of caller</param>
        /// <param name="callingFile">Auto filled file path of caller</param>
        public static void Log(string tag, string message, LogLevel level = LogLevel.Internal,bool highlight = false, [CallerMemberName] string callingMethod = "", [CallerFilePath] string callingFile = "")
        {
            string scriptName = System.IO.Path.GetFileNameWithoutExtension(callingFile);


            LogEntryDto entryDto = new LogEntryDto
            {
                ScriptName = scriptName,
                MethodName = callingMethod,
                Level = level,
                Tag = tag,
                Message = message, 
                Timestamp = DateTime.Now
            };

            string levelColor = GetColorHex(entryDto.Level);
            string highlightColor = "#FA00E4";
            string color = highlight ? highlightColor : levelColor;

            string formatted = $"<color={color}>{entryDto}</color>";

            //Console Logging
            switch (level)
            {
                case LogLevel.Internal:
                    Debug.Log($"{formatted}");
                    break;
                case LogLevel.Info:
                    Debug.Log($"ℹ️ℹ️ℹ️{formatted}");
                    break;
                case LogLevel.Success:
                    Debug.Log($"✅✅✅{formatted}");
                    break;
                case LogLevel.Alert:
                    Debug.LogWarning($"⚠️⚠️⚠️{formatted}");
                    break;
                case LogLevel.Error:
                    Debug.LogError($"🔥🔥🔥{formatted}");
                    break;
                case LogLevel.ByPass:
                    Debug.Log($"⚡⚡⚡{formatted}");
                    break;
                default:
                    Debug.Log($"[UNKNOWN LEVEL] {formatted}");
                    break;
            }

            HandleFileLog(entryDto);

            if (!SuppressFrontendLogs)
                OnLogReceived?.Invoke(entryDto);
        }

        public static void LogTemp(string message)
        {
            Debug.Log($"<color=#00FFFF>{message}</color>");
        }

        private static void HandleFileLog(LogEntryDto entryDto)
        {
            LogFileWriter.AppendToLog(entryDto.ToString());
        }

        public static void TestLog(LogLevel level, bool testAll = false)
        {
            if (testAll)
            {
                Log("Test", "Test Info Log", LogLevel.Info);
                Log("Test", "Test Success Log", LogLevel.Success);
                Log("Test", "Test Alert Log", LogLevel.Alert);
                Log("Test", "Test Error Log", LogLevel.Error);
            }
            else
            {
                Log("Test", $"Test {level.ToString()} Log", level);
            }
        }

        private static string GetColorHex(LogLevel level)
        {
            return level switch
            {
                LogLevel.Info => "#32B5E1",
                LogLevel.Success => "#32D475",
                LogLevel.Alert => "#FFB347",
                LogLevel.Error => "#E94F4F",
                LogLevel.ByPass => "#FF00FF",
                _ => "#CCCCCC"
            };
        }
    }
}
