using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Factory;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Enums;
using KayosTech.ReelDeal.Prototype.Core.Event;
using KayosTech.ReelDeal.Prototype.LogSystem;
using System.Net.Http;
using UnityEngine;
using System;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Bridge
{
    public interface IServiceCommand
    {
        ActionType Type { get; }
        HttpRequestMessage Request { get; }
    }

    public class RegisterDeviceCommand : IServiceCommand
    {
        public ActionType Type => ActionType.RegisterDevice;
        public  HttpRequestMessage Request { get; }

        public RegisterDeviceCommand(HttpRequestMessage request)
        {
            Request = request;
        }
    }

    public static class CommandDispatcher 
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager); // Force static constructor
            EventManager.OnActionDispatched += HandleIncomingActionPayload;
        }

        private static void HandleIncomingActionPayload(IActionPayload payload)
        {
            DevLog.Internal("Action Payload Received");
            var command = ServiceCommandFactory.CreateFrom(payload);
            EventManager.DispatchServiceCommand(command);
        }
    }
}
