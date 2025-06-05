using KayosTech.ReelDeal.Prototype.LogSystem.Payload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem.Backend.Handler
{
    public class SaveToFileUtility : MonoBehaviour
    {
        
        private static readonly string logDirectory = Path.Combine(Application.persistentDataPath, "Logs");

        public static string GenerateLogFilePath()
        {
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            return Path.Combine(logDirectory, $"log_{timestamp}.txt");
        }

        public static string FormatLogs(IReadOnlyList<LogCachePayload> logs)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var log in logs)
            {
                sb.AppendLine(log.Formatted);
            }

            return sb.ToString();
        }

        public static void WriteToFile(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                Debug.Log($"Logs saved to {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveToFileUtility] Failed to save log file: {ex.Message}");
            }
        }
    }
}