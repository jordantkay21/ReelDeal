using System.Collections.Generic;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Manager;
using KayosTech.ReelDeal.Prototype.LogSystem.Backend.Controller;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Frontend;
using KayosTech.ReelDeal.Prototype.LogSystem.DataStructure;
using UnityEngine;
using UnityEngine.Rendering;

namespace KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Backend
{
    public static class LogStorage
    {
        public static class LogCache
        {
            private static readonly List<LogCachePayload> CachedLogs = new();

            public static void Add(LogCachePayload log)
            {
                CachedLogs.Add(log);
            }

            public static IReadOnlyList<LogCachePayload> GetAll() => CachedLogs;

            public static void Clear() => CachedLogs.Clear();
        }
        public static void Initialize()
        {
            Application.quitting += HandleQuit;
            LogRouter.OnDownstreamCommand += AcceptCommand; 
        }
        private static void HandleQuit()
        {
            if (!LogSettingsManager.Instance.shouldSaveLogsOnExit) return;

            var logs = LogCache.GetAll();
            LogFileWriter.WriteLogsToDisk(logs);

            LogCache.Clear();
        }

        public static void AcceptCommand(LogCommandDTO command)
        {
            var logCache = new LogCachePayload(
                command.Type,
                command.Tag,
                command.Message,
                command.CallerScript,
                command.CallerMethod,
                command.SourceSystem,
                command.Timestamp,
                command.PrimaryColor);

            LogCache.Add(logCache);
            DebugToUnityConsole(logCache);
        }

        private static void DebugToUnityConsole(LogCachePayload log)
        {
            switch (log.Type)
            {
                case AppLogType.Error:
                    Debug.LogError(log.ConsoleFormat);
                    break;
                case AppLogType.Alert:
                    Debug.LogWarning(log.ConsoleFormat);
                    break;
                default:
                    Debug.Log(log.ConsoleFormat);
                    break;
            }
        }



    }


}