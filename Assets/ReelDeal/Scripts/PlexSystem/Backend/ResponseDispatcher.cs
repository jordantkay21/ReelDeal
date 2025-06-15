using KayosTech.ReelDeal.Prototype.Core.Event;
using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Bridge;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Factory;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Backend
{
    public static class ResponseDispatcher
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _ = typeof(EventManager); // Force static constructor
            EventManager.OnServiceCommandDispatched += HandleIncomingServiceCommand;
        }

        private static void HandleIncomingServiceCommand(IServiceCommand command)
        {
            DevLog.Highlight($"Service Command Received \n Request: {command.Request}");
        }
    }
}
