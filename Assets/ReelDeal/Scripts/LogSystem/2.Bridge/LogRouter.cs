using System;
using System.Runtime.CompilerServices;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Backend;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Manager;
using KayosTech.ReelDeal.Prototype.LogSystem.Payload;
using KayosTech.ReelDeal.Prototype.LogSystem.Settings;
using KayosTech.Utilities.DebugTools;
using UnityEngine;


namespace KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Frontend
{
    public static class LogRouter
    {
        public static event Action<LogCommandDTO> OnRenderToUI;
        public static event Action<LogCommandDTO> OnCacheLog;

        public static void Initialize()
        {
            DevLog.OnLogReceived += AcceptPayload;
        }

        public static void AcceptPayload(LogActionPayload logAction)
        {

            if (string.IsNullOrWhiteSpace(logAction.message))
            {
                Debug.LogWarning("LogRouter: Rejected empty log command.");
                return;
            }

            var settings = LogSettingsManager.Instance.GetSettingsForType(logAction.type);

            var command = new LogCommandDTO(
                logAction.type,
                logAction.tag,
                logAction.message,
                logAction.callerScript,
                logAction.callerScript,
                logAction.sourceSystem,
                logAction.timestamp,
                settings.primaryColor,
                settings.secondaryColor,
                settings.duration,
                settings.fadeDuration,
                settings.showInUI);

            RouteCommand(command);
        }

        public static void RouteCommand(LogCommandDTO command)
        {
            OnCacheLog?.Invoke(command);
            OnRenderToUI?.Invoke(command);
        }        
    }
}
