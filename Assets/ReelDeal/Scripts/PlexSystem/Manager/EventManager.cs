using KayosTech.ReelDeal.Prototype.LogSystem;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Bridge;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Enums;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Factory;
using KayosTech.ReelDeal.Prototype.SessionConfiguration.PlexLogic.Interfaces;
using UnityEngine;

namespace KayosTech.ReelDeal.Prototype.Core.Event
{
    public static class EventManager
    {
        static EventManager() {}

        #region User Interaction Event
        public delegate void InteractionDispatched(ActionType type, string rawData);

        public static event InteractionDispatched OnInteractionDispatched;

        public static void DispatchInteraction(ActionType type, string rawData = null)
        {
            OnInteractionDispatched?.Invoke(type, rawData);
        }
        #endregion

        #region Action Event

        public delegate void ActionDispatched(IActionPayload payload);

        public static event ActionDispatched OnActionDispatched;

        public static void DispatchActionPayload(IActionPayload payload)
        {
            OnActionDispatched?.Invoke(payload);
        }

        #endregion

        #region Command Event

        public delegate void ServiceCommandDispatched(IServiceCommand command);

        public static event ServiceCommandDispatched OnServiceCommandDispatched;

        public static void DispatchServiceCommand(IServiceCommand command)
        {
            OnServiceCommandDispatched?.Invoke(command);
        }

        #endregion
    }
}
