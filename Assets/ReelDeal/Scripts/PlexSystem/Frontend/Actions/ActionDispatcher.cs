using KayosTech.ReelDeal.Prototype.Core.Event;
using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Enums;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Factory;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Frontend
{
    public class ActionDispatcher : MonoBehaviour
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
}
