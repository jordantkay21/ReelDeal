using System;
using System.Linq;
using System.Runtime.CompilerServices;
using KayosTech.ReelDeal.Prototype.LogSystem.DataStructure;
using KayosTech.ReelDeal.Prototype.LogSystem.Settings;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.LogSystem
{
    public static class DevLog
    {
        public static event Action<LogActionPayload> OnLogReceived;

        public static void Internal(string message, string tag = "", [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFile = "")
        {
            Log(message, AppLogType.Internal, tag, callingMethod, callingFile);
        }

        public static void Highlight(string message, string tag = "", [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFile = "")
        {
            Log(message, AppLogType.Highlight, tag, callingMethod, callingFile);
        }

        public static void Info(string message, string tag = "", [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFile = "")
        {
            Log(message, AppLogType.Info, tag, callingMethod, callingFile);
        }

        public static void Success(string message, string tag = "",
            [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFile = "")
        {
            Log(message, AppLogType.Success, tag, callingMethod, callingFile);
        }

        public static void Warning(string message, string tag = "",
            [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFile = "")
        {
            Log(message, AppLogType.Alert, tag, callingMethod, callingFile);
        }

        public static void Error(string message, string tag = "",
            [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFile = "")
        {
            Log(message, AppLogType.Error, tag, callingMethod, callingFile);
        }

        public static void Urgent(string message, string tag = "",
            [CallerMemberName] string callingMethod = "",
            [CallerFilePath] string callingFile = "")
        {
            Log(message, AppLogType.Urgent, tag, callingMethod, callingFile);
        }

        private static void Log(string message, AppLogType type, string tag,
            string callingMethod, string callingFile)
        {
            string scriptName = System.IO.Path.GetFileNameWithoutExtension(callingFile);
            string systemName = Utilities.ExtractSystemNamespace(scriptName);

            var actionPayload = new LogActionPayload(
                type,
                tag,
                message,
                scriptName,
                callingMethod,
                systemName,
                DateTime.Now
            );

            if (!LogSystemBuffer.IsReady)
            {
                LogSystemBuffer.Enqueue(actionPayload);
            }
            else
            {
                OnLogReceived?.Invoke(actionPayload);
            }
        }

        public static void ForceRoute(LogActionPayload payload)
        {
            OnLogReceived?.Invoke(payload);
        }
    }
}
