using KayosTech.ReelDeal.Prototype.LogSystem.Payload;
using System;
using System.Collections.Generic;
using KayosTech.ReelDeal.Prototype.LogSystem.Backend.Handler;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem.Backend.Controller
{
    public static class LogFileWriter
    {
        public static void WriteLogsToDisk(IReadOnlyList<LogCachePayload> logs)
        {
            if (logs == null || logs.Count == 0) return;

            string formattedList = SaveToFileUtility.FormatLogs(logs);
            string filePath = SaveToFileUtility.GenerateLogFilePath();
            SaveToFileUtility.WriteToFile(filePath, formattedList);
        }
    }
}