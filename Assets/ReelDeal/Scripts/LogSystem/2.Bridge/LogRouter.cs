using System;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Manager;
using KayosTech.ReelDeal.Prototype.LogSystem.Payload;
using UnityEngine;


namespace KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Frontend
{
    public static class LogRouter
    {
        public static event Action<LogCommandDTO> OnUpstreamCommand;
        public static event Action<LogCommandDTO> OnDownstreamCommand;

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
            OnDownstreamCommand?.Invoke(command);
            
            if(command.ShowInUI)
                OnUpstreamCommand?.Invoke(command);
        }        
    }
}
