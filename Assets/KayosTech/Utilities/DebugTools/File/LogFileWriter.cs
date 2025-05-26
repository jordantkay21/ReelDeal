using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KayosTech.Utilities.DebugTools
{
    /// <summary>
    /// Static helper for handling log file creation, writing, and cleanup.
    /// </summary>
    public static class LogFileWriter
    {
        private const int MaxLogFiles = 10;

        private static string logFilePath;
        private static StringBuilder logBuffer = new();

        public static void ClearAllLogs()
        {
            string folder = GetLogFolderPath();

            if (!Directory.Exists(folder))
            {
                LogRouter.Log("Logger","No logs to clear.");
                return;
            }

            string[] files = Directory.GetFiles(folder, "log_*.txt");

            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                    LogRouter.Log($"Logger", $"Deleted log: {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    LogRouter.Log("Logger", $"Failed to delete {Path.GetFileName(file)}: {ex.Message}");
                }
            }

            LogRouter.Log("Logger", $"Cleared {files.Length} log files.");
        }

        public static void InitializeLogFile()
        {
            string folder = GetLogFolderPath();
            Directory.CreateDirectory(folder);

            EnforceLogLimit(folder);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            logFilePath = Path.Combine(folder, $"log_{timestamp}.txt");

            AppendToLog($"[SESSION START] {DateTime.Now}");
        }

        public static void AppendToLog(string message)
        {
            logBuffer.AppendLine(message);
        }

        public static void SaveLogFile()
        {
            if (string.IsNullOrEmpty(logFilePath))
            {
                Debug.LogWarning("[LocalLogger] SaveLogFile called but logFilePath is null or empty.");
                return;
            }

            File.WriteAllText(logFilePath, logBuffer.ToString());
        }

        /// <summary>
        /// Returns the appropriate log folder path depending on platform
        /// </summary>
        private static string GetLogFolderPath()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                $"{Application.productName}Logs");
#else
            return Path.Combine(Application.persistentDataPath, $"{Application.productName}Logs");
#endif
        }

        private static void EnforceLogLimit(string logDirectory)
        {
            var logFiles = new DirectoryInfo(logDirectory)
                .GetFiles("log_*.txt")
                .OrderBy(f => f.CreationTimeUtc)
                .ToList();

            LogRouter.SuppressFrontendLogs = true;

            while (logFiles.Count > MaxLogFiles)
            {
                FileInfo oldest = logFiles[0];
                
                try
                {
                    oldest.Delete();
                    LogRouter.Log("Logger", $"Deleted old log: {oldest.Name}");
                }
                catch (Exception ex)
                {
                    LogRouter.Log("Logger", $"Failed to delete {oldest.Name}: {ex.Message}");
                }

                logFiles.RemoveAt(0);
            }

            LogRouter.SuppressFrontendLogs = false;
        }
    }
}