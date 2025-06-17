using System;
using KayosTech.ReelDeal.Prototype.Core;
using KayosTech.ReelDeal.Prototype.LogSystem;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic
{
    public static class ActionDispatcher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager); // Force static constructor
            EventManager.OnInteractionDispatched += HandleInteraction;
        }
        private static void HandleInteraction(ActionType type, string rawData)
        {
            DevLog.Internal("User Interaction Received", "DataFlow");
            var payload = ActionPayloadFactory.Create(type, rawData);
            EventManager.DispatchActionPayload(payload);
        }
    }

    public static class CommandDispatcher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager); // Force static constructor
            EventManager.OnActionDispatched += HandleIncomingActionPayload;
            EventManager.OnResponsePayloadDispatched += HandleIncomingResponsePayload;
        }

        private static void HandleIncomingActionPayload(IActionPayload action)
        {
            DevLog.Internal("Action Payload Received");
            var command = ServiceCommandFactory.CreateFrom(action);
            EventManager.DispatchServiceCommand(command);
        }

        private static void HandleIncomingResponsePayload(IResponsePayload response)
        {
            DevLog.Internal($"Response Payload Received \n Response: {response?.ToString() ?? "[null response]"}");
            var display = DisplayCommandFactory.CreateFrom(response);
            EventManager.DispatchDisplayCommand(display);
            
        }
    }

    public static class ResponseDispatcher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager); // Force static constructor
            EventManager.OnServiceCommandDispatched += HandleIncomingServiceCommand;
        }

        private static async void HandleIncomingServiceCommand(IServiceCommand command)
        {
            DevLog.Internal($"Service Command Received \n Request: {command.Request}");

            try
            {
                string rawData = await ServiceUtility.ExecuteAsync(command.Request);
                var response = ResponsePayloadFactory.Create(command.Action, rawData);
                EventManager.DispatchResponsePayload(response);
            }
            catch (Exception ex)
            {
                DevLog.Error($"Command Execution Failed: {ex.Message}");
            }

        }
    }

    public static class DisplayDispatcher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager); // Force static constructor
            EventManager.OnDisplayCommandDispatched += HandleIncomingDisplayCommand;
        }

        private static void HandleIncomingDisplayCommand(IDisplayCommand display)
        {
            DevLog.Highlight($"Display Command Received \n {display}");
        }
    }
}

