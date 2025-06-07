using System;
using KayosTech.ReelDeal.Prototype.LogSystem.Bridge.Manager;
using KayosTech.ReelDeal.Prototype.LogSystem.DataStructure;
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

            if (string.IsNullOrWhiteSpace(logAction.Message))
            {
                Debug.LogWarning("LogRouter: Rejected empty log command.");
                return;
            }

            var settings = LogSettingsManager.Instance.GetSettingsForType(logAction.Type);

            var command = new LogCommandDTO(
                logAction.Type,
                logAction.Tag,
                logAction.Message,
                logAction.CallerScript,
                logAction.CallerScript,
                logAction.SourceSystem,
                logAction.Timestamp,
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
